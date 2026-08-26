using System.Security.Claims;

namespace SimpleSellBooks_API.RateLimiting
{
    public class RateLimitHelper
    {
        public static string GetPartitionKey(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User
                    .FindFirst(ClaimTypes.NameIdentifier)
                    ?.Value;
                return $"ip:{(!string.IsNullOrEmpty(userId) ? userId : "unknownUser")}";
            }

            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            return $"ip:{(!string.IsNullOrEmpty(ipAddress) ? ipAddress : "unknownIP")}";
        }
    }
}
