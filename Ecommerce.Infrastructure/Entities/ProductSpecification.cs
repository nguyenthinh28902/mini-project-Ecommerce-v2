using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class ProductSpecification
{
    public int Id { get; set; }

    public int ProductVariantId { get; set; }

    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public string? FilterKey { get; set; }

    public string SpecKey { get; set; } = null!;

    public string DisplayValue { get; set; } = null!;

    public string? Unit { get; set; }

    public decimal? NumericValue { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
