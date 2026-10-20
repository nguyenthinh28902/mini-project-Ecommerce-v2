using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class ApplicationPermission
{
    public int Id { get; set; }

    public string PermissionKey { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
