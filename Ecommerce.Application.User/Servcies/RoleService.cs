using AutoMapper;
using Ecommerce.Application.User.Interfaces;
using Ecommerce.Infrastructure.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Infrastructure.Persistence.UnitOfWork;
using Microsoft.Extensions.Logging;
using Project.Common.Enum.Entities;
using Project.Common.Libraries;
using Project.Common.Models;
using Project.Common.Models.Dtos.ApplicationPermission;
using Project.Common.Models.Dtos.User;
using Project.Common.Servcies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Servcies
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public RoleService(ILogger<RoleService> logger, IRoleRepository roleRepository, ICurrentUserService currentUserService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// hàm get role theo id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<RoleDto>> GetRoleById(int Id, CancellationToken cancellationToken = default) {

            if (!_currentUserService.HasPermission(PermissionKey.system_role_key_view.ToString())) return Result<RoleDto>.Failure("Msg_RoleManagement_Forbidden", (int)HttpStatusCode.Forbidden);
            var entity = await _roleRepository.GetRoleByIdAsync(Id ,cancellationToken);
            if (entity == null) return Result<RoleDto>.Failure("Msg_RoleNotFound", (int)HttpStatusCode.NotFound);
            var dto = _mapper.Map<RoleDto>(entity);
            return Result<RoleDto>.Success(dto);
        }

        /// <summary>
        /// lấy danh sách
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<RoleDto>>> GetRoles(RoleQuery query, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.HasPermission(PermissionKey.system_role_key_view.ToString())) return Result<RoleDto>.Failure("Msg_RoleManagement_Forbidden", (int)HttpStatusCode.Forbidden);
            var entiies = await _roleRepository.GetRolesAsync(query, cancellationToken);
            var dtos = _mapper.Map<List<RoleDto>>(entiies);
            var result = new PagedCustomResult<RoleDto>()
            {
                Items = dtos,
                PageIndex = entiies.PageIndex,
                PageSize = entiies.PageSize,
                TotalItems = entiies.TotalItems
            };
            return result;
        }
        public async Task<Result<RoleCreateOrUpdateDto>> CreateOrUpdateRoleAsync(RoleCreateOrUpdateDto model, CancellationToken cancellationToken = default)
        {
            if(model == null) return Result<RoleCreateOrUpdateDto>.Failure("Msg_RoleCreateManagement_BadRequest", (int)HttpStatusCode.BadRequest);
            if (model.Id > 0)
            {
              return  UpdateRoleAsync(model, cancellationToken);
            }
            else
            {
              return CreateRoleAsync(model, cancellationToken);
            }
        }

        private async Task<Result<RoleCreateOrUpdateDto>> CreateRoleAsync(RoleCreateOrUpdateDto model, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.HasPermission(PermissionKey.system_role_key_create.ToString())) return Result<RoleCreateOrUpdateDto>.Failure("Msg_RoleCreateManagement_Forbidden", (int)HttpStatusCode.Forbidden);
            if(model == null) return Result<RoleCreateOrUpdateDto>.Failure("Msg_RoleCreateManagement_BadRequest", (int)HttpStatusCode.BadRequest);
            var entity = await _roleRepository.GetRoleByNameAsync(model.Name,cancellationToken);
            if(entity != null) return Result<RoleCreateOrUpdateDto>.Failure("Msg_RoleCreateManagement_Conflict", (int)HttpStatusCode.Conflict);
            try
            {
                entity = _mapper.Map<Role>(model);
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await _roleRepository.AddRoleAsync(entity, cancellationToken);
                var record = await _unitOfWork.CommitAsync(cancellationToken);
                if (record <= 0)
                {
                    _logger.LogWarning("không có record thay đổi khi thực hiện lưu Role. {name}", nameof(CreateRoleAsync));
                    _logger.LogWarning("Role data: {RoleCreateOrUpdateDto}", FuncHelper.JsonModleToString(model));
                    return Result<RoleCreateOrUpdateDto>.Failure("Msg_Role_Save_Error", (int)HttpStatusCode.InternalServerError);
                }
                model = _mapper.Map<RoleCreateOrUpdateDto>(entity);
                return Result<RoleCreateOrUpdateDto>.Success(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm mới role. {name}", nameof(CreateRoleAsync));
                _logger.LogError(ex, ex.Message);
                return Result<RoleCreateOrUpdateDto>.Failure("Msg_Role_Save_Error", (int)HttpStatusCode.InternalServerError);
            }
        }
        
        private async Task<Result<RoleCreateOrUpdateDto>> UpdateRoleAsync(RoleCreateOrUpdateDto model, CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.HasPermission(PermissionKey.system_role_key_update.ToString())) return Result<RoleCreateOrUpdateDto>.Failure("Msg_RoleCreateManagement_Forbidden", (int)HttpStatusCode.Forbidden);
            if (model == null) return Result<RoleCreateOrUpdateDto>.Failure("Msg_RoleCreateManagement_BadRequest", (int)HttpStatusCode.BadRequest);
            var entity = await _roleRepository.GetRoleByIdAsync(model.Id, cancellationToken);
            if (entity == null) return Result<RoleCreateOrUpdateDto>.Failure("Msg_RoleNotFound", (int)HttpStatusCode.NotFound);
            try
            {
                entity.Name = model.Name;
                entity.NameDisplay = model.NameDisplay;
                entity.NormalizedName = model.NormalizedName;
                entity.Description = model.Description;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                _roleRepository.UpdateRoleAsync(entity);
                var record = await _unitOfWork.CommitAsync(cancellationToken);
                if (record <= 0)
                {
                    _logger.LogWarning("không có record thay đổi khi thực hiện lưu Role. {name}", nameof(UpdateRoleAsync));
                    _logger.LogWarning("Role data: {UpdateRoleAsync}", FuncHelper.JsonModleToString(model));
                    return Result<RoleCreateOrUpdateDto>.Failure("Msg_Role_Save_Error", (int)HttpStatusCode.InternalServerError);
                }
                model = _mapper.Map<RoleCreateOrUpdateDto>(entity);
                return Result<RoleCreateOrUpdateDto>.Success(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật role. {name}", nameof(CreateRoleAsync));
                _logger.LogError(ex, ex.Message);
                return Result<RoleCreateOrUpdateDto>.Failure("Msg_Role_Save_Error", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
