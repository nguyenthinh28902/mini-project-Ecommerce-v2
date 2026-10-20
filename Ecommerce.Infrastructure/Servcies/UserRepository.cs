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
    public class UserRepository : IUserRepository
    {
        private readonly EcommerceDbContext _dbContext;
        public UserRepository(EcommerceDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
        }
        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == email, cancellationToken);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetClaimUsersAsync(int UserId,CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserClaims.Where(x => x.UserId == UserId).Select(x => x.ClaimValue).ToListAsync(cancellationToken);
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
        }
        public void DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
        }
    }
}
