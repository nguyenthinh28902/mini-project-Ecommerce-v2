using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models.Dtos.User
{

    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NameDisplay { get; set; } = null!;
        public string NormalizedName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class RoleCreateOrUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string NameDisplay { get; set; } = null!;
        public string NormalizedName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class RoleQuery : Page
    {
        public string Keywork { get; set; } = string.Empty;
        // --- Sắp xếp (Sorting) ---
        public string? SortBy { get; set; } = "CreatedAt"; // Trường cần sắp xếp (ví dụ: "Name", "CreatedAt", "GroupName")
        public bool IsDescending { get; set; } = true;     // Hướng sắp xếp: true = giảm dần (DESC), false = tăng dần (ASC)
    }
}
