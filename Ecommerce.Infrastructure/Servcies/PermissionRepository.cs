using Ecommerce.Infrastructure.Dbcontext;
using Ecommerce.Infrastructure.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Project.Common.Models.Dtos.ApplicationPermission;
using System.Linq.Dynamic.Core;

namespace Ecommerce.Infrastructure.Servcies
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly EcommerceDbContext _dbContext;
        public PermissionRepository(EcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasPermissionAsync(string permissionName)
        {
            return await _dbContext.ApplicationPermissions.AnyAsync(p => p.Name == permissionName);
        }
        public async Task AddPermissionAsync(ApplicationPermission permission, CancellationToken cancellationToken = default)
        {
            await _dbContext.ApplicationPermissions.AddAsync(permission, cancellationToken);
        }
        public async Task<ApplicationPermission?> GetPermissionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApplicationPermissions.FindAsync(new object[] { id }, cancellationToken);
        }
        public async Task<ApplicationPermission?> GetPermissionByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApplicationPermissions.FirstOrDefaultAsync(p => p.PermissionKey == key, cancellationToken);
        }
        public async Task<IEnumerable<ApplicationPermission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApplicationPermissions.ToListAsync(cancellationToken);
        }
        public void UpdatePermission(ApplicationPermission permission)
        {
            _dbContext.ApplicationPermissions.Update(permission);
        }
        public void DeletePermission(ApplicationPermission  permission)
        {
            _dbContext.ApplicationPermissions.Remove(permission);
        }
        public async Task<Project.Common.Models.PagedResult<ApplicationPermission>> GetApplicationPermissionsAsync(ApplicationPermissionQuery queryModel, CancellationToken cancellationToken = default)
        {

            var iQuery = _dbContext.ApplicationPermissions.AsQueryable();
           
            if (!string.IsNullOrEmpty(queryModel.Keywork)) {
                iQuery = iQuery.Where(x => x.Name.Contains(queryModel.Keywork) || x.GroupName.Contains(queryModel.Keywork) || (x.Description != null && x.Description.Contains(queryModel.Keywork)));
            }
            if (!string.IsNullOrEmpty(queryModel.SortBy))
            {
                // Xác định hướng sắp xếp: "GroupName desc" hoặc "CreatedAt asc"
                string direction = queryModel.IsDescending ? "desc" : "asc";
                string orderByString = $"{queryModel.SortBy} {direction}";

                // EF Core sẽ tự động hiểu và dịch ra câu lệnh SQL ORDER BY tương ứng
                iQuery = iQuery.OrderBy(orderByString);
            }
            else
            {
                iQuery = iQuery.OrderByDescending(x => x.Id); // Sắp xếp mặc định theo Id nếu không có SortBy
            }
            
            var TotalItem = await iQuery.CountAsync(cancellationToken);
            var list = await EntityServices.PageEntityAsync(iQuery, queryModel.PageSize, queryModel.PageIndex, cancellationToken);

            return new Project.Common.Models.PagedResult<ApplicationPermission>
            {
                Items = list,
                PageIndex = queryModel.PageIndex,
                PageSize = queryModel.PageSize,
                TotalItems = TotalItem
            };  
        }
    }
}
