using System.Text;
using CarGalary.Application.Validations;
using CarGalary.Domain.Entities;
using CarGalary.Infrastructure.Auth;
using CarGalary.Infrastructure.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CarGalary.Admin.Api;
using CarGalary.Admin.Api.Hubs;
using CarGalary.Application.ErrorCatalog;
using System.Security.Claims;
using CarGalary.Application.Dtos.Auth;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // frontend origin
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
// =======================
// Services
// =======================

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiErrorResponseFilter>();
    })
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<CargalaryValidatorClass>();
        fv.AutomaticValidationEnabled = true;
    });

// Swagger / OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IErrorCatalogService, ErrorCatalogService>();

// Authorization & Authentication
builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        var response = new ApiErrorResponse(
            "Too many requests. Please try again later.",
            StatusCodes.Status429TooManyRequests);
        await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
    };

    options.AddPolicy("AuthLoginPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: GetClientIp(httpContext),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    options.AddPolicy("AuthRegisterPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: GetClientIp(httpContext),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(10),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
                AutoReplenishment = true
            }));
});

builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider, CarGalary.Admin.Api.Security.PermissionPolicyProvider>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, CarGalary.Admin.Api.Security.PermissionAuthorizationHandler>();


builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs/requests") ||
                     path.StartsWithSegments("/hubs/quotations") ||
                     path.StartsWithSegments("/hubs/cars")))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = async context =>
            {
                var userIdRaw = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userIdRaw, out var userId))
                {
                    return;
                }

                using var scopedProvider = context.HttpContext.RequestServices.CreateScope();
                var dbContext = scopedProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    return;
                }

                var utcNow = DateTime.UtcNow;
                if (!user.LastActivityAt.HasValue || (utcNow - user.LastActivityAt.Value).TotalMinutes >= 2)
                {
                    user.LastActivityAt = utcNow;
                    await dbContext.SaveChangesAsync();
                }
            }
        };
    });

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

  builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Customize password rules
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6; // set your minimum length
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


// Dependency Injection
builder.Services.AddCarGalaryDependencies();

// =======================
// Build App
// =======================
var app = builder.Build();
// 3. Use CORS
app.UseCors("AllowAngular");
// =======================
// Middleware Pipeline
// =======================

// 1. Global exception handling (FIRST)
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. HTTPS
app.UseHttpsRedirection();

// 2.1 Serve files from wwwroot
app.UseStaticFiles();

// 3. Routing
app.UseRouting();
app.UseRateLimiter();




// Swagger (DEV only)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Test endpoint
app.MapGet("/api/version", () => Results.Ok(new { vers = "1.0" }));
// 4. Authentication / Authorization
app.UseAuthentication();
app.UseAuthorization();
// 6. Endpoints
app.MapControllers();
app.MapHub<RequestHub>("/hubs/requests");
app.MapHub<RequestHub>("/hubs/quotations");
app.MapHub<CarHub>("/hubs/cars");




app.Run();

static string GetClientIp(HttpContext httpContext)
{
    if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
    {
        var first = forwardedFor.ToString().Split(',')[0].Trim();
        if (!string.IsNullOrWhiteSpace(first))
        {
            return first;
        }
    }

    return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}
