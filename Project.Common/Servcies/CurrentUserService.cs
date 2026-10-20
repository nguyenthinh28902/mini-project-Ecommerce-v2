using Microsoft.AspNetCore.Http;
using Project.Common.Enum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Servcies
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        // JWT: sub
        // Lấy ID người dùng từ claim NameIdentifier
        public string? UserId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // JWT: preferred_username hoặc ClaimTypes.Name tùy theo cách bạn map lúc tạo token
        public string? Username => User?.FindFirst(ClaimTypes.Name)?.Value
                                ?? User?.FindFirst("preferred_username")?.Value;

        // JWT: email
        public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

        // JWT: ClaimTypes.Role
        public IEnumerable<string> Roles =>
            User?.FindAll(ClaimTypes.Role).Select(c => c.Value)
            ?? Enumerable.Empty<string>();

        // Khớp với key "Permission" khi add vào danh sách claim của token
        public IEnumerable<string> Permissions =>
            User?.FindAll("Permission").Select(c => c.Value)
            ?? Enumerable.Empty<string>();

        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission) || Permissions.Contains(PermissionKey.system_full_access.ToString());
        }
    }
}
