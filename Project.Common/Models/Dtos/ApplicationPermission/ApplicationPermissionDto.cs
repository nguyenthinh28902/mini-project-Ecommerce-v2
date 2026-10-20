using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models.Dtos.ApplicationPermission
{
    public class ApplicationPermissionDto
    {
        public int Id { get; set; }

        public string PermissionKey { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string GroupName { get; set; } = null!;

        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
    public class ApplicationPermissionQuery  : Page
    {
        public string Keywork { get; set; } = string.Empty;
        // --- Sắp xếp (Sorting) ---
        public string? SortBy { get; set; } = "CreatedAt"; // Trường cần sắp xếp (ví dụ: "Name", "CreatedAt", "GroupName")
        public bool IsDescending { get; set; } = true;     // Hướng sắp xếp: true = giảm dần (DESC), false = tăng dần (ASC)
    }
}
