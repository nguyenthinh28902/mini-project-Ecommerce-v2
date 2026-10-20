using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models.Dtos.User
{
    public class UserClaimDto
    {
        public int Id { get; set; }
        public int WorkLocationId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Danh sách các vai trò của user (có thể tạo một RoleDto nhỏ gọn riêng nếu cần)
        public List<string> Roles { get; set; } = new();

        // Danh sách các key quyền hạn phân rã (ví dụ: ["System_FullAccess", "Order_Create",...])
        public List<string> PermissionKeys { get; set; } = new();
    }
}
