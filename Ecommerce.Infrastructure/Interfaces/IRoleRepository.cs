using Ecommerce.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces
{
    public interface IRoleRepository
    {
        Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken = default);
        Task AddRoleAsync(Role role, CancellationToken cancellationToken = default);
        Task<Role?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Role>> GetRoleByUserAsync(int UserId, CancellationToken cancellationToken = default);

    }
}
