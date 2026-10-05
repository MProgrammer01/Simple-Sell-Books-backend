using System.Security.Claims;

namespace SimpleSellBooks_API.helper_methods
{
    public class clsHelperMethods
    {
        public static int? GetCurrentUserId(HttpContext httpContext)
        {
            var userIdClaim = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        public static string? GetCurrentRole(HttpContext httpContext)
        {
            var userRole = httpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            return string.IsNullOrEmpty(userRole) ? null : userRole;
        }
    }
}
