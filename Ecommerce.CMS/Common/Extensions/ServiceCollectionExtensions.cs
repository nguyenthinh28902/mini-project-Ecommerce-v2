using Ecommerce.Application.User.Dependency;
using Project.Common.Servcies;

namespace Ecommerce.CMS.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceCollectionExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMappingViewService();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddApplicationUserServices(configuration);
            return services;
        }
      
    }
}
