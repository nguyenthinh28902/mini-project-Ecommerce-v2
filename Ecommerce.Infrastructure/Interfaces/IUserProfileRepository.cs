using Project.Common.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<InforClaim?> GetInforSignIn(int userId);
    }
}
