using Ecommerce.Application.User.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ecommerce.CMS.Common.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/dang-nhap-he-thong"; // Đường dẫn chuyển hướng khi chưa đăng nhập
                options.AccessDeniedPath = "/tu-choi-truy-cap"; // Đường dẫn khi không đủ quyền
                // Cấu hình thời gian sống và tính chất của Cookie tại đây
                options.ExpireTimeSpan = TimeSpan.FromHours(10); // Thời gian hết hạn (10 tiếng)
                options.SlidingExpiration = true; // Tự động gia hạn nếu user tiếp tục hoạt động trong thời gian cookie còn hiệu lực
                options.Cookie.HttpOnly = true; // Bảo mật cookie, chống tấn công XSS
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ gửi cookie qua giao thức HTTPS
                options.Events = new CookieAuthenticationEvents {
                    OnValidatePrincipal = async context =>
                    {
                        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthServcie>();
                        await authService.ValidateSecurityStampAsync();
                    }
                }
            });
            return services;
        }
    }
}
