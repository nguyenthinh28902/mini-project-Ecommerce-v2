using AutoMapper;
using Ecommerce.Application.User.Interfaces;
using Ecommerce.Infrastructure.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Infrastructure.Persistence.UnitOfWork;
using Microsoft.Extensions.Logging;
using Project.Common.Enum.Entities;
using Project.Common.Exceptions;
using Project.Common.Libraries;
using Project.Common.Models;
using Project.Common.Models.Dtos.ApplicationPermission;
using Project.Common.Servcies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Servcies
{
    public class PermissionService : IPermissionService
    { 
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionService> _logger;
        public PermissionService(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, IMapper mapper, ILogger<PermissionService> logger)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get danh sach ke quyền theo diều kiện
        /// </summary>
        /// <param name="permissionQuery"></param>
        /// <returns></returns>
        public async Task<PagedCustomResult<ApplicationPermissionDto>> GetApplicationPermissionAsync(ApplicationPermissionQuery permissionQuery, CancellationToken cancellationToken = default)
        {
           var checkPermission =  _currentUserService.HasPermission(PermissionKey.system_full_access.ToString());
            if (!checkPermission)
            {
               throw new UnauthorizedException("Message_Forbidden_ReadData");
            }
            permissionQuery.PageSize = (permissionQuery.PageSize <= 0) ? 20 : permissionQuery.PageSize;
            permissionQuery.PageIndex = ( permissionQuery.PageIndex <= 0) ? 1 : permissionQuery.PageIndex;

            var applicationPermissions = await _permissionRepository
                .GetApplicationPermissionsAsync(permissionQuery);
           var dtos =  _mapper.Map<List<ApplicationPermissionDto>>(applicationPermissions.Items);

            var result = new PagedCustomResult<ApplicationPermissionDto>()
            {
                Items = dtos,
                PageIndex = permissionQuery.PageIndex,
                PageSize = permissionQuery.PageSize,
                TotalItems = applicationPermissions.TotalItems
            };
            return result;
        }

        /// <summary>
        /// Lấy key quyền theo mã id
        /// </summary>
        /// <param name="permissionId">id quyền</param>
        /// <returns>ApplicationPermissionDto</returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Result<ApplicationPermissionDto>> GetApplicationPermissionByIdAsync(int permissionId, CancellationToken cancellationToken = default)
        {
            var checkPermission = _currentUserService.HasPermission(PermissionKey.system_permission_key_view.ToString());
            if (!checkPermission)
            {
                return Result<ApplicationPermissionDto>.Failure("Message_Forbidden_ReadData", (int) HttpStatusCode.Forbidden);
            }
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
            {
                return Result<ApplicationPermissionDto>.Failure("Msg_Permission_NotFound", (int) HttpStatusCode.NotFound);
            }
            var dto = _mapper.Map<ApplicationPermissionDto>(permission);
            var result = Result<ApplicationPermissionDto>.Success(dto);
            return result;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật thông tin key quyền
        /// </summary>
        /// <param name="permissionDto"></param>
        /// <returns></returns>
        public async Task<Result<ApplicationPermissionDto>> CreateOrUpdateApplicationPermissionAsync(ApplicationPermissionCreateOrUpdateDto permissionDto, CancellationToken cancellationToken = default)
        {
          
            if (permissionDto.Id > 0)
            {  
                return await UpdateApplicationPermissionAsync(permissionDto, cancellationToken);
            }
            else
            {
                return await CreateApplicationPermissionAsync(permissionDto, cancellationToken);
            }
        }
       /// <summary>
       /// Thêm mới key quyền
       /// </summary>
       /// <param name="permissionDto"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
       /// <exception cref="UnauthorizedException"></exception>
        private async Task<Result<ApplicationPermissionDto>> CreateApplicationPermissionAsync(ApplicationPermissionCreateOrUpdateDto permissionDto, CancellationToken cancellationToken = default)
        {
            var checkPermission = _currentUserService.HasPermission(PermissionKey.system_permission_key_create.ToString());
            if (!checkPermission)
            {
                return Result<ApplicationPermissionDto>.Failure("Message_Forbidden_CreateOrUpdateData");
            }
            var permission = _mapper.Map<ApplicationPermission>(permissionDto);
            try
            {
                var existingPermission = await _permissionRepository.GetPermissionByKeyAsync(permission.PermissionKey, cancellationToken);
                if (existingPermission != null)
                {
                    return Result<ApplicationPermissionDto>.Failure("Msg_Permission_Key_Already_Exists",(int) HttpStatusCode.Conflict);
                }
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await _permissionRepository.AddPermissionAsync(permission);
                var record = await _unitOfWork.CommitAsync(cancellationToken);
                if (record <= 0)
                {
                    _logger.LogWarning("không có record thay đổi khi thực hiện lưu permission.");
                    _logger.LogWarning("Permission data: {PermissionData}", FuncHelper.JsonModleToString(permission));
                    return Result<ApplicationPermissionDto>.Failure("Msg_Permission_Save_Error", (int) HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating permission.");
                _logger.LogError(ex, ex.Message);
                return Result<ApplicationPermissionDto>.Failure("Msg_Permission_Save_Error", (int) HttpStatusCode.InternalServerError);
            }
            
            var resultDto = _mapper.Map<ApplicationPermissionDto>(permission);
            return Result<ApplicationPermissionDto>.Success(resultDto, "Msg_Permission_Save_Success");
        }
        /// <summary>
        /// Cập nhật key quyền
        /// </summary>
        /// <param name="permissionDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        private async Task<Result<ApplicationPermissionDto>> UpdateApplicationPermissionAsync(ApplicationPermissionCreateOrUpdateDto permissionDto, CancellationToken cancellationToken = default)
        {
            var checkPermission = _currentUserService.HasPermission(PermissionKey.system_permission_key_update.ToString());
            if (!checkPermission)
            {
                 return Result<ApplicationPermissionDto>.Failure("Message_Forbidden_CreateOrUpdateData", (int) HttpStatusCode.Forbidden);
            }
            var permission = _mapper.Map<ApplicationPermission>(permissionDto);
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                var entity = await _permissionRepository.GetPermissionByIdAsync(permission.Id, cancellationToken);
                if (entity == null)
                {
                    return Result<ApplicationPermissionDto>.Failure("Msg_Permission_NotFound", (int) HttpStatusCode.NotFound);
                }
                permission.IsDelete = entity.IsDelete;
                permission.CreatedAt = entity.CreatedAt;
                _permissionRepository.UpdatePermission(permission);
                var record = await _unitOfWork.CommitAsync(cancellationToken);
                if (record <= 0)
                {
                    _logger.LogWarning("không có record thay đổi khi thực hiện lưu permission.");
                    _logger.LogWarning("Permission data: {PermissionData}", FuncHelper.JsonModleToString(permission));
                    return Result<ApplicationPermissionDto>.Failure("Msg_Permission_Save_Error", (int) HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating permission.");
                _logger.LogError(ex, ex.Message);
                return Result<ApplicationPermissionDto>.Failure("Msg_Permission_Save_Error", (int) HttpStatusCode.InternalServerError);
            }
           
            var resultDto = _mapper.Map<ApplicationPermissionDto>(permission);
            return Result<ApplicationPermissionDto>.Success(resultDto, "Msg_Permission_Save_Success");
        }
    }
}
