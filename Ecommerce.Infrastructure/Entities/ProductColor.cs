using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class ProductColor
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ColorName { get; set; } = null!;

    public string? ColorHex { get; set; }

    public bool? IsDefault { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ColorImage> ColorImages { get; set; } = new List<ColorImage>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
