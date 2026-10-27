using Ecommerce.Application.User.Interfaces;
using Ecommerce.Infrastructure.Dbcontext;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Project.Common.Enum.Entities;
using Project.Common.Libraries;
using Project.Common.Models;
using Project.Common.Models.Dtos.Auth;
using Project.Common.Models.Dtos.User;
using System.Security.Claims;

namespace Ecommerce.Application.User.Servcies
{
    public class AuthServcie : IAuthServcie
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<AuthServcie> _logger;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthServcie(IUnitOfWork unitOfWork, IUserRepository userRepo, IRoleRepository roleRepo, ILogger<AuthServcie> logger, IUserProfileRepository userProfileRepo,
             IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _logger = logger;
            _userProfileRepo = userProfileRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<UserClaimDto>> ProcessSignInAsync(SignInDto signInDto)
        {
            var validation = await Validation(signInDto);
            if (validation.IsSuccess && validation.Data != null) await EstablishUserSessionAsync(validation.Data);
            return validation;
        }

        public async Task<Result<UserClaimDto>> Validation(SignInDto signInDto)
        {
            if (signInDto == null || signInDto.Id <= 0 || string.IsNullOrEmpty(signInDto.Password))
            {
                _logger.LogWarning("Đăng nhập thất bại do thiếu thông tin. Email: {Email}, Password provided: {HasPassword}",
                signInDto?.Id,
                !string.IsNullOrEmpty(signInDto?.Password));
                return Result<UserClaimDto>.Failure("Message_Validation_Failure_UserIdAndPasswordRequired");
            }

            var user = await _userRepo.GetUserByIdAsync(signInDto.Id);
            if (user == null)
            {
                return Result<UserClaimDto>.Failure("Message_Validation_Failure_UserId");
            }
            if (user.LockoutEnabled == false || user.IsDelete == true)
            {
                _logger.LogWarning("Tài khoản {UserId} đã bị khóa không thể đăng nhập.", signInDto.Id);
                return Result<UserClaimDto>.Failure("Message_Validation_Failure_AccountLocked");
            }
            var isPasswordValid = PasswordHelper.VerifyPassword(user.PasswordHash, signInDto.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Đăng nhập thất bại do mật khẩu không hợp lệ. UserId: {UserId}", signInDto.Id);
                return Result<UserClaimDto>.Failure("Message_Validation_Failure_Password");
            }
            var roles = await _roleRepo.GetRoleByUserAsync(user.Id);
            var inforSignIn = await _userProfileRepo.GetInforSignIn(user.Id);
            if (inforSignIn == null)
            {
                if (roles.FirstOrDefault(x => x.Name == PermissionRole.SuperAdmin.ToString()) != null)
                {
                    inforSignIn = new InforClaim() { DisplayName = user.UserName, Email = string.Empty, PhoneNumber = string.Empty };
                }
                else
                {
                    _logger.LogWarning("Không tìm thấy thông tin nhận sự trong hệ thống userId là {UserId}", user.Id);
                    return Result<UserClaimDto>.Failure("Message_UserInfo_NotFound");
                }

            }
            var userClaim = new UserClaimDto
            {
                Id = user.Id,
                Email = inforSignIn.Email,
                DisplayName = inforSignIn.DisplayName,
                PhoneNumber = inforSignIn.PhoneNumber,
                SecurityStamp = user.SecurityStamp,
                WorkLocationId = user.WorkLocationId,
                Roles = roles.Select(x => x.Name).ToList(),
                PermissionKeys = roles.SelectMany(r => r.RoleClaims.Select(rc => rc.ClaimValue)).Distinct().ToList()
            };

            var claimUser = await _userRepo.GetClaimUsersAsync(user.Id);
            userClaim.PermissionKeys.AddRange(claimUser);

            return Result<UserClaimDto>.Success(userClaim);
        }

        public async Task EstablishUserSessionAsync(UserClaimDto userDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Email, userDto.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, userDto.DisplayName ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, userDto.PhoneNumber ?? string.Empty),
                new Claim("SecurityStamp", userDto.SecurityStamp ?? string.Empty),
                new Claim("WorkLocationId", userDto.WorkLocationId.ToString())
            };

            if (userDto.Roles != null)
            {
                foreach (var role in userDto.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            if (userDto.PermissionKeys != null)
            {
                foreach (var permission in userDto.PermissionKeys)
                {
                    claims.Add(new Claim("Permission", permission));
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var httpContext = _httpContextAccessor.HttpContext;

            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                IsPersistent = true
            };
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);
        }
        public async Task ValidateSecurityStampAsync(CookieValidatePrincipalContext context)
        {
            var userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tokenStamp = context.Principal.FindFirst("SecurityStamp")?.Value;

            var dbContext = context.HttpContext.RequestServices.GetRequiredService<EcommerceDbContext>();
            var isSurrentStamp = await _userRepo.IsSecurityStampValidAsync(userId, tokenStamp);

            if (!isSurrentStamp)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
