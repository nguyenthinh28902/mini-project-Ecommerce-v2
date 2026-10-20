using Ecommerce.Application.User.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Dependency
{
    public static class MappingService
    {
        public static IServiceCollection AddMappingService(this IServiceCollection services)
        {

            services.AddAutoMapper(cfg => {
               cfg.AddProfile<PermissionProfile>();
            });
            return services;
        }
    }
}
