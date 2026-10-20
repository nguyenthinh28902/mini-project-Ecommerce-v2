using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models.ViewModels.ApplicationPermission
{
    public class PermissionSearchViewModel : Page
    {
        [Display(Name = "View_Search")]
        public string Keywork { get; set; } = string.Empty;
        // --- Sắp xếp (Sorting) ---
        [Display(Name = "View_SortBy")]
        public string? SortBy { get; set; } = "CreatedAt"; // Trường cần sắp xếp (ví dụ: "Name", "CreatedAt", "GroupName")
        [Display(Name = "View_IsDescending")]
        public bool IsDescending { get; set; } = true;     // Hướng sắp xếp: true = giảm dần (DESC), false = tăng dần (ASC)
    }
    public enum ApplicationPermissionSortField
    {
        [Display(Name = "Permission_Sort_Id")]
        Id,
        [Display(Name = "Permission_Sort_PermissionKey")]
        PermissionKey,
        [Display(Name = "Permission_Sort_Name")]
        Name,
        [Display(Name = "Permission_Sort_GroupName")]
        GroupName,
        [Display(Name = "Permission_Sort_CreatedAt")]
        CreatedAt,
        [Display(Name = "Permission_Sort_UpdatedAt")]
        UpdatedAt
    }
    public class ApplicationPermissionViewModel
    {
        [Display(Name = "Permission_Field_Id")]
        public int Id { get; set; }

        [Display(Name = "Permission_Field_PermissionKey")]
        public string PermissionKey { get; set; } = null!;

        [Display(Name = "Permission_Field_Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Permission_Field_GroupName")]
        public string GroupName { get; set; } = null!;

        [Display(Name = "Permission_Field_Description")]
        public string? Description { get; set; }

        [Display(Name = "Permission_Field_CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Permission_Field_UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
    public class ApplicationPermissionUpsertViewModel
    {
        [Display(Name = "Permission_Field_Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Validation_Required")]
        [StringLength(100, ErrorMessage = "Validation_StringLength_Max")]
        [Display(Name = "Permission_Field_PermissionKey")]
        public string PermissionKey { get; set; } = null!;
        [Required(ErrorMessage = "Validation_Required")]
        [StringLength(150, ErrorMessage = "Validation_StringLength_Max")]
        [Display(Name = "Permission_Field_Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Validation_Required")]
        [StringLength(100, ErrorMessage = "Validation_StringLength_Max")]
        [Display(Name = "Permission_Field_GroupName")]
        public string GroupName { get; set; } = null!;
        [StringLength(500, ErrorMessage = "Validation_StringLength_Max")]

        [Display(Name = "Permission_Field_Description")]
        public string? Description { get; set; }

        [Display(Name = "Permission_Field_IsDelete")]
        public bool? IsDelete { get; set; }

        [Display(Name = "Permission_Field_CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Permission_Field_UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        public bool IsNew => Id <= 0;
        public bool IsAction { get; set; } = false;
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
