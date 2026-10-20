using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class Order
{
    public int Id { get; set; }

    public string OrderCode { get; set; } = null!;

    public int? CustomerId { get; set; }

    public int? UserId { get; set; }

    public int? WorkLocationId { get; set; }

    public string FulfillmentType { get; set; } = null!;

    public string OrderSource { get; set; } = null!;

    public string? WorkLocationCode { get; set; }

    public string? WorkLocationName { get; set; }

    public string? WorkLocationAddress { get; set; }

    public string? UserFullName { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPhoneNumber { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? CustomerEmail { get; set; }

    public string CustomerPhone { get; set; } = null!;

    public string ReceiverName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? ShippingFee { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public string? Note { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
