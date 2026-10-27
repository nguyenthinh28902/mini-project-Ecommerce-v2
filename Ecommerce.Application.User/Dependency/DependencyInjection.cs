using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Infrastructure.Dependency;
using Ecommerce.Application.User.Interfaces;
using Ecommerce.Application.User.Servcies;

namespace Ecommerce.Application.User.Dependency
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationUserServices(this IServiceCollection services, IConfiguration configuration) {

            services.AddInfrastructureServices(configuration);
            services.AddMappingService();
            services.AddScoped<IAuthServcie, AuthServcie>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }
    }
}
