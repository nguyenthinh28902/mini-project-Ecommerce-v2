using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class Inventory
{
    public int Id { get; set; }

    public int WarehouseId { get; set; }

    public int ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public int ReservedQuantity { get; set; }

    public int? AvailableQuantity { get; set; }

    public int? MinStockLevel { get; set; }

    public int? MaxStockLevel { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
