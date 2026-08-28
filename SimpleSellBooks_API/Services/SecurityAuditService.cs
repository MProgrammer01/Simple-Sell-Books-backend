using SimpleSellBooks_DataLayer.Models;

namespace SimpleSellBooks_API.Services
{
    public class SecurityAuditService : ISecurityAuditService
    {
        private readonly SimpleSellBooksDbContext _context;

        public SecurityAuditService(SimpleSellBooksDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(
            string? eventType,
            HttpContext context,
            int? userId = null,
            SecurityAction? action = null,
            int? statusCode = null,
            string? targetType = null,
            string? targetId = null,
            string? details = null)
        {
            var auditLog = new SecurityAuditLog
            {
                UserId = userId,
                EventType = eventType ?? "known",
                Action = action.ToString(),
                Endpoint = context.Request.Path,
                HttpMethod = context.Request.Method,
                StatusCode = statusCode,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                TargetType = targetType,
                TargetId = targetId,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _context.SecurityAuditLogs.Add(auditLog);

            await _context.SaveChangesAsync();
        }
    }
}
