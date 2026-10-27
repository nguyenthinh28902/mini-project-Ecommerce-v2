using Ecommerce.Infrastructure.Dbcontext;
using Ecommerce.Infrastructure.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Project.Common.Models;
using Project.Common.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Servcies
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EcommerceDbContext _dbContext;

        public RoleRepository(EcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles.AnyAsync(r => r.Name == roleName, cancellationToken);
        }

        public async Task AddRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            await _dbContext.Roles.AddAsync(role, cancellationToken);
        }
        public async Task<Role?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }
        public async Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetRoleByUserAsync(int UserId ,CancellationToken cancellationToken = default)
        {
           return await _dbContext.Roles.Include(x => x.UserRoles.Where(y => y.UserId == UserId)).Include(x => x.RoleClaims).ToListAsync();
        }

        public void UpdateRoleAsync(Role role)
        {
            _dbContext.Roles.Update(role);
        }
        public void DeleteRoleAsync(Role role)
        {
            _dbContext.Roles.Remove(role);
        }

        public async Task<PagedCustomResult<Role>> GetRolesAsync(RoleQuery queryModel, CancellationToken cancellationToken = default)
        {
            var iQuery = _dbContext.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(queryModel.Keywork))
            {
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
            return PagedCustomResult<Role>
            {
                Items = list,
                PageIndex = queryModel.PageIndex,
                PageSize = queryModel.PageSize,
                TotalItems = TotalItem
            };
        }
    }
}
