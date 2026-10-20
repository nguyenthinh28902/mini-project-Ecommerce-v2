using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models.Dtos.ApplicationPermission
{
    public class ApplicationPermissionCreateOrUpdateDto
    {
        public int Id { get; set; }

        public string PermissionKey { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string GroupName { get; set; } = null!;

        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
