using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class OrderUpdateItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int? ProductVariantId { get; set; }

    public string ProductName { get; set; } = null!;

    public string VariantName { get; set; } = null!;

    public string Sku { get; set; } = null!;

    public string? ColorName { get; set; }

    public string? ColorHex { get; set; }

    public string? ThumbnailUrl { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal Price { get; set; }

    public decimal? SalePrice { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual OrderUpdateRequest Order { get; set; } = null!;
}
