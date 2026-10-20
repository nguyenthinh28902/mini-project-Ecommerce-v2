using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class ProductVariant
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ColorId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Sku { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal? SalePrice { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal? DiscountPercent { get; set; }

    public bool? IsAllowStackingPromo { get; set; }

    public bool? IsDefault { get; set; }

    public int? VariantImageId { get; set; }

    public bool? IsDelete { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProductColor Color { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<InventoryTransactionItem> InventoryTransactionItems { get; set; } = new List<InventoryTransactionItem>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();
}
