namespace SimpleSellBooks_API.Services
{
    public enum SecurityEventTypeAndAction
    {
        LoginSucceeded,
        LoginFailed,

        SignUpSucceeded,
        SignUpFailed,

        RefreshSucceeded,
        RefreshFailed,

        AuthenticationRequired, // 401
        AuthorizationDenied,    // 403

        AdminAction,

        SensitiveResourceAccessed,

        SuspiciousActivityDetected,

        RateLimitExceeded
    }

    public enum SecurityAction
    {
        Login,

        Add,

        Delete,

        Update,

        Logout,

        RefreshToken,

        RateLimit,

        AccessDenied
    }
}
