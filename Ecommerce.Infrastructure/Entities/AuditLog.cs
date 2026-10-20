using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class AuditLog
{
    public int Id { get; set; }

    public string IpAddress { get; set; } = null!;

    public string EntityName { get; set; } = null!;

    public string EntityId { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string? ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }
}
