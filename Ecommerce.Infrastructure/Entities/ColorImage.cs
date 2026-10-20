using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class ColorImage
{
    public int Id { get; set; }

    public int ColorId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    public bool? IsThumbnail { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProductColor Color { get; set; } = null!;
}
