using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleSellBooks_API.Authorization;
using SimpleSellBooks_API.RateLimiting;
using SimpleSellBooks_API.Services;
using SimpleSellBooks_DataLayer.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ===============================
// CORS Configuration
// ===============================

builder.Services.AddCors(options =>
{
    options.AddPolicy("StoreBooksApiCorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7217",
                "http://localhost:5215"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ===============================
// JWT Authentication Configuration
// ===============================


// Register authentication services in the dependency injection container.
// JwtBearerDefaults.AuthenticationScheme tells ASP.NET Core that
// JWT Bearer authentication will be the default authentication method.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters define how incoming JWTs will be validated.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Ensures the token was issued by a trusted issuer.
            ValidateIssuer = true,


            // Ensures the token is intended for this API (audience check).
            ValidateAudience = true,


            // Ensures the token has not expired.
            ValidateLifetime = true,


            // Ensures the token signature is valid and was signed by the API.
            ValidateIssuerSigningKey = true,


            // The expected issuer value (must match the issuer used when creating the JWT).
            ValidIssuer = "StoreBookApi",


            // The expected audience value (must match the audience used when creating the JWT).
            ValidAudience = "StoreApiBooks",


            // The secret key used to validate the JWT signature.
            // This must be the same key used when generating the token.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"))
        };
    });

// ===============================
// Authorization Configuration With DI
// ===============================

builder.Services.AddSingleton<IAuthorizationHandler, OwnerOrAdminHandler>();

builder.Services.AddScoped<ISecurityAuditService, SecurityAuditService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerOrAdmin", policy =>
        policy.Requirements.Add(new OwnerOrAdminRequirement()));
});

builder.Services.AddScoped<AuthorizationMiddlewareResultHandler>();

builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler>(sp =>
    {
        var defaultHandler =
            sp.GetRequiredService<AuthorizationMiddlewareResultHandler>();

        var auditService =
            sp.GetRequiredService<ISecurityAuditService>();

        return new SecurityAuthorizationMiddlewareResultHandler(
            defaultHandler,
            auditService);
    });
// Register authorization services.
// This enables attributes like [Authorize] and role-based authorization.
builder.Services.AddAuthorization();

// ===============================
// RateLimiting Configuration
// ===============================
builder.Services.AddSingleton<AdaptiveRateLimitService>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        context =>
            RateLimitPartition.GetSlidingWindowLimiter(
                partitionKey: "global",
                factory: _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 6,
                    QueueLimit = 0
                }));
    
    options.AddPolicy("PerIpOrUser", context =>
    {
        var service = context.RequestServices
        .GetRequiredService<AdaptiveRateLimitService>();

        var permitLimit = service.GetPermitLimit(context);

        return RateLimitPartition.GetSlidingWindowLimiter(
            RateLimitHelper.GetPartitionKey(context),
            _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueLimit = 0
            });
    });
    
    options.AddPolicy("CreatePolicy", context =>
    {
        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: RateLimitHelper.GetPartitionKey(context),
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 25,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueLimit = 0
            });
    });

    options.AddPolicy("UpdatePolicy", context =>
    {
        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: RateLimitHelper.GetPartitionKey(context),
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueLimit = 0
            });
    });

    options.AddPolicy("DeletePolicy", context =>
    {
        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: RateLimitHelper.GetPartitionKey(context),
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueLimit = 0
            });
    });

    options.AddPolicy("AuthPolicy", context =>
    {
        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: RateLimitHelper.GetPartitionKey(context),
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueLimit = 0
            });
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        var httpContext = context.HttpContext;

        var auditService = httpContext.RequestServices
            .GetRequiredService<ISecurityAuditService>();

        var policyName = httpContext
            .GetEndpoint()?
            .Metadata
            .GetMetadata<EnableRateLimitingAttribute>()?
            .PolicyName
            ?? "GlobalLimiter";

        await auditService.LogAsync(
            SecurityEventTypeAndAction.RateLimitExceeded.ToString(),
            httpContext,
            statusCode: StatusCodes.Status429TooManyRequests,
            action: SecurityAction.RateLimit,
            details: $"Request rejected by policy {policyName} because the rate limit was exceeded."
        );

        httpContext.Response.StatusCode =
            StatusCodes.Status429TooManyRequests;
    };
});

// Register controller support.
builder.Services.AddControllers();


// ===============================
// Swagger Configuration
// ===============================


// Enables Swagger endpoint discovery.
builder.Services.AddEndpointsApiExplorer();


// Enables Swagger UI for testing and documentation.
// Register Swagger generator and customize its behavior.
builder.Services.AddSwaggerGen(options =>
{
    // ===============================
    // 1) Define the JWT Bearer security scheme
    // ===============================
    //
    // This tells Swagger that our API uses JWT Bearer authentication
    // through the HTTP Authorization header.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // The name of the HTTP header where the token will be sent.
        Name = "Authorization",


        // Indicates this is an HTTP authentication scheme.
        Type = SecuritySchemeType.Http,


        // Specifies the authentication scheme name.
        // Must be exactly "Bearer" for JWT Bearer tokens.
        Scheme = "Bearer",


        // Optional metadata to describe the token format.
        BearerFormat = "JWT",


        // Specifies that the token is sent in the request header.
        In = ParameterLocation.Header,


        // Text shown in Swagger UI to guide the user.
        Description = "Enter: Bearer {your JWT token}"
    });


    // ===============================
    // 2) Require the Bearer scheme for secured endpoints
    // ===============================
    //
    // This tells Swagger that endpoints protected by [Authorize]
    // require the Bearer token defined above.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },


            // No scopes are required for JWT Bearer authentication.
            // This array is empty because JWT does not use OAuth scopes here.
            new string[] {}
        }
    });
});

// ===============================
// Set DI Of DBContext
// ===============================
builder.Services.AddDbContext<SimpleSellBooksDbContext>();

// Build the application.
// After this point, services are frozen and middleware is configured.
var app = builder.Build();

// ===============================
// HTTP Request Pipeline
// ===============================

// Enable Swagger only in development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

app.UseRateLimiter();

//Use Of CORS
app.UseCors("StoreBooksApiCorsPolicy");

// IMPORTANT:
// Authentication middleware must run BEFORE authorization middleware.
// Authentication identifies the user.
// Authorization decides what the user is allowed to do.
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes (e.g., /api/Person, /api/Auth).
app.MapControllers().RequireRateLimiting("PerIpOrUser");

// Start the application.
app.Run();
