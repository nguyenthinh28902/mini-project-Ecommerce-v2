using Ecommerce.Infrastructure.Dbcontext;
using Ecommerce.Infrastructure.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            return await _dbContext.Roles.FindAsync(new object[] { id }, cancellationToken);
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
    }
}
