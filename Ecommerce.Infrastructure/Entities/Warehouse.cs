using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class Warehouse
{
    public int Id { get; set; }

    public int WorkLocationId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<InventoryTransaction> InventoryTransactionDestinationWarehouses { get; set; } = new List<InventoryTransaction>();

    public virtual ICollection<InventoryTransaction> InventoryTransactionSourceWarehouses { get; set; } = new List<InventoryTransaction>();

    public virtual WorkLocation WorkLocation { get; set; } = null!;
}
