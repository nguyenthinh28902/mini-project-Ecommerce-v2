using Ecommerce.Application.User.Profiles;
using Ecommerce.CMS.Common.Mapping;

namespace Ecommerce.CMS.Common.Extensions
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMappingViewService(this IServiceCollection services) {
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<PermissionViewProfile>();
            });
            return services;

        }
    }
}
