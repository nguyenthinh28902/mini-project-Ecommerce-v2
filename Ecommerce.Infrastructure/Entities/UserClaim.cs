using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class UserClaim
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ClaimType { get; set; } = null!;

    public string ClaimValue { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
