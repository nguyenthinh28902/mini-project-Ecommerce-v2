using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Project.Common.Models;
using Project.Common.Servcies;
using System.Text.Json;

namespace Ecommerce.CMS.ViewComponents
{
    public class MenuNavViewComponent : ViewComponent
    {
        private readonly IStringLocalizer<Project.Common.Models.SharedResource> _localizer;
        private readonly IWebHostEnvironment _env;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;

        public MenuNavViewComponent(IWebHostEnvironment env, IStringLocalizer<SharedResource> localizer, 
            ICurrentUserService currentUserService,
            ICacheService cacheService)
        {
            _env = env;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public IViewComponentResult Invoke()
        {
            List<MenuItemModel> menuList = new List<MenuItemModel>();
            string cacheKey = $"{_currentUserService.SecurityStamp}_MenuNav";
            menuList = _cacheService.Get<List<MenuItemModel>>(cacheKey);
            if (menuList == null || menuList.Count == 0)
            {
                // Đường dẫn tới file json (ví dụ đặt tại wwwroot/data/menu.json hoặc App_Data)
                string path = Path.Combine(_env.WebRootPath, "data", "menu.json");

                if (System.IO.File.Exists(path))
                {
                    string jsonString = System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path) : "[]";
                    menuList = JsonSerializer.Deserialize<List<MenuItemModel>>(jsonString) ?? new List<MenuItemModel>();

                }
                menuList = TranslateMenuItems(menuList);
                _cacheService.Set<List<MenuItemModel>>(cacheKey, menuList);
            }
            return View(menuList);
        }

        private List<MenuItemModel> TranslateMenuItems(List<MenuItemModel> items)
        {
            if (items == null) return new List<MenuItemModel>();
            var itemsCopy = new List<MenuItemModel>(items); // Tạo bản sao để tránh lỗi khi xóa phần tử trong vòng lặp
            foreach (var item in items)
            {
                var hasPermission = string.IsNullOrEmpty(item.PermissionKey) ||
                                     _currentUserService.HasPermission(item.PermissionKey);

                if (hasPermission)
                {
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
                else
                {
                    // Nếu không có quyền, loại bỏ item này khỏi danh sách
                    itemsCopy.Remove(item);
                }
            }
            return itemsCopy;
        }
    }
}
