using Ecommerce.Infrastructure.Entities;
using Project.Common.Models;
using Project.Common.Models.Dtos.ApplicationPermission;


namespace Ecommerce.Infrastructure.Interfaces
{
    public interface IPermissionRepository
    {
        Task<bool> HasPermissionAsync(string permissionName);
        Task AddPermissionAsync(ApplicationPermission permission, CancellationToken cancellationToken = default);
        Task<ApplicationPermission?> GetPermissionByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ApplicationPermission?> GetPermissionByKeyAsync(string key, CancellationToken cancellationToken = default);
        Task<IEnumerable<ApplicationPermission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
        void UpdatePermission(ApplicationPermission permission);
        void DeletePermission(ApplicationPermission permission);
        Task<PagedCustomResult<ApplicationPermission>> GetApplicationPermissionsAsync(ApplicationPermissionQuery queryModel, CancellationToken cancellationToken = default);
    }
}
