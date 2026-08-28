using System;
using System.Collections.Generic;

namespace SimpleSellBooks_DataLayer.Models;

public partial class SecurityAuditLog
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string EventType { get; set; } = null!;

    public string? Action { get; set; }

    public string? Endpoint { get; set; }

    public string? HttpMethod { get; set; }

    public int? StatusCode { get; set; }

    public string? IpAddress { get; set; }

    public string? TargetType { get; set; }

    public string? TargetId { get; set; }

    public string? Details { get; set; }

    public DateTime Timestamp { get; set; }
}
