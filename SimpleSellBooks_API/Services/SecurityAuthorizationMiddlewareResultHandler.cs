using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using SimpleSellBooks_API.helper_methods;
using System.Security.Claims;

namespace SimpleSellBooks_API.Services
{
    public class SecurityAuthorizationMiddlewareResultHandler
    : IAuthorizationMiddlewareResultHandler
    {
        private readonly IAuthorizationMiddlewareResultHandler _defaultHandler;
        private readonly ISecurityAuditService _auditService;

        public SecurityAuthorizationMiddlewareResultHandler(
            IAuthorizationMiddlewareResultHandler defaultHandler,
            ISecurityAuditService auditService)
        {
            _defaultHandler = defaultHandler;
            _auditService = auditService;
        }

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Challenged)
            {
                await _auditService.LogAsync(
                    SecurityEventTypeAndAction.AuthenticationRequired.ToString(),
                    context,
                    statusCode: StatusCodes.Status401Unauthorized,
                    action: SecurityAction.AccessDenied,
                    details: "Anonymous user attempted to access a protected endpoint."
                );
            }
            else if (authorizeResult.Forbidden)
            {
                await _auditService.LogAsync(
                    SecurityEventTypeAndAction.AuthorizationDenied.ToString(),
                    context,
                    userId: clsHelperMethods.GetCurrentUserId(context),
                    statusCode: StatusCodes.Status403Forbidden,
                    action: SecurityAction.AccessDenied,
                    details: "Authenticated user is not authorized to access the endpoint."
                );
            }

            await _defaultHandler.HandleAsync(
                next,
                context,
                policy,
                authorizeResult);
        }
    }
}
