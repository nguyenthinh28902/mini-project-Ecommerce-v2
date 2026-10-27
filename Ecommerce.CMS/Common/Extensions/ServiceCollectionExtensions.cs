using Ecommerce.Application.User.Dependency;
using Project.Common.Servcies;

namespace Ecommerce.CMS.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceCollectionExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = 1024;
            });
            services.AddMappingViewService();
            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddApplicationUserServices(configuration);
            return services;
        }
      
    }
}
