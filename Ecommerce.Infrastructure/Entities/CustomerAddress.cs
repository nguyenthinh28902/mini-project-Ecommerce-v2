using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class CustomerAddress
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string ReceiverName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string ProvinceName { get; set; } = null!;

    public string DistrictName { get; set; } = null!;

    public string WardName { get; set; } = null!;

    public string DetailAddress { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
