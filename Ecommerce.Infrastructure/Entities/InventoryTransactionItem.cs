using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class InventoryTransactionItem
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public int ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Note { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual InventoryTransaction Transaction { get; set; } = null!;
}
