using Ecommerce.Infrastructure.Dbcontext;
using Ecommerce.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Project.Common.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Servcies
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly EcommerceDbContext _dbContext;
        public UserProfileRepository(EcommerceDbContext dbContext) { 
            _dbContext = dbContext; 
        }


        public async Task<InforClaim?> GetInforSignIn(int userId) {
        
            return await _dbContext.UserProfiles.Where(x => x.UserId == userId).Select(x => new InforClaim
            {
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                DisplayName = x.DisplayName
            }).FirstOrDefaultAsync();

        }
    }
}
