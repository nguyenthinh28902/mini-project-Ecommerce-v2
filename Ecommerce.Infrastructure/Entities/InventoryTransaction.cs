using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class InventoryTransaction
{
    public int Id { get; set; }

    public string TransactionCode { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public int? SourceWarehouseId { get; set; }

    public int? DestinationWarehouseId { get; set; }

    public int? ReferenceId { get; set; }

    public int CreatedByUserId { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual Warehouse? DestinationWarehouse { get; set; }

    public virtual ICollection<InventoryTransactionItem> InventoryTransactionItems { get; set; } = new List<InventoryTransactionItem>();

    public virtual Warehouse? SourceWarehouse { get; set; }
}
