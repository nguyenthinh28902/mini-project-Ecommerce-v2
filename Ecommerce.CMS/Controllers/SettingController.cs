using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.CMS.Controllers
{
    public class SettingController : Controller
    {
        public SettingController()
        {
        }

        [HttpPost("ngon-ngu")]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            // Đặt cookie ngôn ngữ theo chuẩn của ASP.NET Core RequestLocalization
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Quay lại trang trước đó
            return LocalRedirect(returnUrl);
        }
    }
}
