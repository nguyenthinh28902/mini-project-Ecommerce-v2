using AutoMapper;
using Ecommerce.Application.User.Interfaces;
using Ecommerce.Infrastructure.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Infrastructure.Persistence.UnitOfWork;
using Project.Common.Enum.Entities;
using Project.Common.Exceptions;
using Project.Common.Models;
using Project.Common.Models.Dtos.ApplicationPermission;
using Project.Common.Servcies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Servcies
{
    public class RolePermissionService : IRolePermissionService
    { 
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        public RolePermissionService(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get danh sach ke quyền theo diều kiện
        /// </summary>
        /// <param name="permissionQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<ApplicationPermissionDto>> GetApplicationPermissionAsync(ApplicationPermissionQuery permissionQuery, CancellationToken cancellationToken = default)
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

            var result = new PagedResult<ApplicationPermissionDto>()
            {
                Items = dtos,
                PageIndex = permissionQuery.PageIndex,
                PageSize = permissionQuery.PageSize,
                TotalItems = applicationPermissions.TotalItems
            };
            return result;
        }

        public async Task<Result<ApplicationPermissionDto>> GetApplicationPermissionByIdAsync(int permissionId, CancellationToken cancellationToken = default)
        {
            var checkPermission = _currentUserService.HasPermission(PermissionKey.system_permission_key_view.ToString());
            if (!checkPermission)
            {
                throw new UnauthorizedException("Message_Forbidden_ReadData");
            }
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
            {
                throw new NotFoundException($"Permission with ID {permissionId} not found.");
            }
            var dto = _mapper.Map<ApplicationPermissionDto>(permission);
            var result = Result<ApplicationPermissionDto>.Success(dto);
            return result;
        }

        public async Task<Result<ApplicationPermissionDto>> CreateOrUpdateApplicationPermissionAsync(ApplicationPermissionCreateOrUpdateDto permissionDto, CancellationToken cancellationToken = default)
        {
            var checkPermission = _currentUserService.HasPermission(PermissionKey.system_permission_key_create.ToString()) ||
                                  _currentUserService.HasPermission(PermissionKey.system_permission_key_update.ToString());
            if (!checkPermission)
            {
                throw new UnauthorizedException("Message_Forbidden_CreateOrUpdateData");
            }
            var permission = _mapper.Map<ApplicationPermission>(permissionDto);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (permission.Id > 0)
            {
                _permissionRepository.UpdatePermission(permission);
            }
            else
            {
                await _permissionRepository.AddPermissionAsync(permission);
            }
            var record =  await _unitOfWork.CommitAsync(cancellationToken);
            if (record <= 0)
            {
                return Result<ApplicationPermissionDto>.Failure("Msg_Permission_Save_Error");
            }
            var resultDto = _mapper.Map<ApplicationPermissionDto>(permission);
            return Result<ApplicationPermissionDto>.Success(resultDto, "Msg_Permission_Save_Success");
        }
    }
}
