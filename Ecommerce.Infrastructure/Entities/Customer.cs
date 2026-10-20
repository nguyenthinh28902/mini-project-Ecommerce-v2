using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? CustomerTierId { get; set; }

    public decimal? TotalSpent { get; set; }

    public int? LoyaltyPoints { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

    public virtual CustomerTier? CustomerTier { get; set; }

    public virtual ICollection<CustomerWishlist> CustomerWishlists { get; set; } = new List<CustomerWishlist>();
}
