using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class CustomerTier
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal MinSpent { get; set; }

    public decimal? DiscountPercent { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
