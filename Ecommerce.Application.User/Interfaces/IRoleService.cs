using Project.Common.Models;
using Project.Common.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Interfaces
{
    public interface IRoleService
    {
        Task<Result<RoleDto>> GetRoleById(int Id, CancellationToken cancellationToken = default);

        Task<Result<List<RoleDto>>> GetRoles(RoleQuery query, CancellationToken cancellationToken = default);

        Task<Result<RoleCreateOrUpdateDto>> CreateOrUpdateRoleAsync(RoleCreateOrUpdateDto model, CancellationToken cancellationToken = default);
    }
}
