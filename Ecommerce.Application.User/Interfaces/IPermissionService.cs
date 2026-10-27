using Project.Common.Models;
using Project.Common.Models.Dtos.ApplicationPermission;

namespace Ecommerce.Application.User.Interfaces
{
    public interface IPermissionService
    {
        Task<PagedCustomResult<ApplicationPermissionDto>> GetApplicationPermissionAsync(ApplicationPermissionQuery permissionQuery, CancellationToken cancellationToken = default);
        Task<Result<ApplicationPermissionDto>> GetApplicationPermissionByIdAsync(int permissionId, CancellationToken cancellationToken = default);
        Task<Result<ApplicationPermissionDto>> CreateOrUpdateApplicationPermissionAsync(ApplicationPermissionCreateOrUpdateDto permissionDto, CancellationToken cancellationToken = default);
    }
}
