namespace SimpleSellBooks_API.RateLimiting
{
    public class AdaptiveRateLimitService
    {
        public int GetPermitLimit(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                return 30;
            }

            if (context.User.IsInRole("Admin"))
            {
                return 500;
            }

            return 200;
        }
    }
}
