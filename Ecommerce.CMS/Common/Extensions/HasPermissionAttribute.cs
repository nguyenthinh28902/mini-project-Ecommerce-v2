using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Project.Common.Enum.Entities;

namespace Ecommerce.CMS.Common.Extensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(string permissionKey) : base(typeof(PermissionAuthorizationFilter))
        {
            Arguments = new object[] { permissionKey };
        }

        private class PermissionAuthorizationFilter : IAuthorizationFilter
        {
            private readonly string _permissionKey;

            public PermissionAuthorizationFilter(string permissionKey)
            {
                _permissionKey = permissionKey;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;

                // Kiểm tra xem user đã đăng nhập chưa
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    context.Result = new ChallengeResult(); // Chưa đăng nhập -> Chuyển hướng về trang Login
                    return;
                }

                // Kiểm tra xem Claims của user có chứa Permission Key yêu cầu không
                // (Lưu ý: Lúc tạo Claims ở hàm EstablishUserSessionAsync bạn đã add claim type là "Permission")
                var hasPermission = user.Claims.Any(c => c.Type == "Permission" && (c.Value == PermissionKey.system_full_access.ToString() || c.Value == _permissionKey));

                if (!hasPermission)
                {
                    // Không đủ quyền -> Chuyển hướng đến trang báo lỗi Access Denied hoặc trả về Forbidden
                    context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                }
            }
        }
    }
}
