using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class WorkLocation
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string LocationType { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ProvinceName { get; set; } = null!;

    public string DistrictName { get; set; } = null!;

    public string? Phone { get; set; }

    public int? ManagerUserId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? ManagerUser { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
