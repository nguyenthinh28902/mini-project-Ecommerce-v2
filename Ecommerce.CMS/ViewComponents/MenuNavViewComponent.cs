using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Project.Common.Models;
using System.Text.Json;

namespace Ecommerce.CMS.ViewComponents
{
    public class MenuNavViewComponent : ViewComponent
    {
        private readonly IStringLocalizer<Project.Common.Models.SharedResource> _localizer;
        private readonly IWebHostEnvironment _env;

        public MenuNavViewComponent(IWebHostEnvironment env, IStringLocalizer<SharedResource> localizer)
        {
            _env = env;
            _localizer = localizer;
        }

        public IViewComponentResult Invoke()
        {
            // Đường dẫn tới file json (ví dụ đặt tại wwwroot/data/menu.json hoặc App_Data)
            string path = Path.Combine(_env.WebRootPath, "data", "menu.json");

            List<MenuItemModel> menuList = new List<MenuItemModel>();

            if (System.IO.File.Exists(path))
            {
                string jsonString = System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path) : "[]";
                menuList = JsonSerializer.Deserialize<List<MenuItemModel>>(jsonString) ?? new List<MenuItemModel>();

            }
            menuList = TranslateMenuItems(menuList);
            return View(menuList); // Truyền model vào View của Component
        }

        private List<MenuItemModel> TranslateMenuItems(List<MenuItemModel> items)
        {
            if (items == null) return new List<MenuItemModel>();

            foreach (var item in items)
            {
                // Dịch title của item hiện tại
                if (!string.IsNullOrEmpty(item.Title))
                {
                    item.Title = _localizer[item.Title];
                }

                // Nếu có cấp con, gọi lại chính hàm này (đệ quy)
                if (item.Children != null && item.Children.Any())
                {
                    TranslateMenuItems(item.Children);
                }
            }
            return items;
        }
    }
}
