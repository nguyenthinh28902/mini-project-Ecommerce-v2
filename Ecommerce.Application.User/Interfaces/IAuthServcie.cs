using Microsoft.AspNetCore.Authentication.Cookies;
using Project.Common.Models;
using Project.Common.Models.Dtos.Auth;
using Project.Common.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Interfaces
{
    public interface IAuthServcie
    {
        Task<Result<UserClaimDto>> ProcessSignInAsync(SignInDto signInDto);
        Task<Result<UserClaimDto>> Validation(SignInDto signInDto);
        Task EstablishUserSessionAsync(UserClaimDto userDto);
        Task ValidateSecurityStampAsync(CookieValidatePrincipalContext context);
    }
}
