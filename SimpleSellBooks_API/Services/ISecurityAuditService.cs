namespace SimpleSellBooks_API.Services
{
    public interface ISecurityAuditService
    {
        Task LogAsync(
            string? eventType,
            HttpContext context,
            int? userId = null,
            SecurityAction? action = null,
            int? statusCode = null,
            string? targetType = null,
            string? targetId = null,
            string? details = null);
    }
}
