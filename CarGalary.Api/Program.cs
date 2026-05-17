using System.Text;
using CarGalary.Api;
using CarGalary.Application.Validations;
using CarGalary.Domain.Entities;
using CarGalary.Infrastructure.Auth;
using CarGalary.Infrastructure.Context;
using CarGalary.Application.ErrorCatalog;
using CarGalary.Application.Dtos.Auth;
using ElmahCore;
using ElmahCore.Mvc;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<IErrorCatalogService, ErrorCatalogService>();
builder.Services.AddElmah<XmlFileErrorLog>(options =>
{
    options.Path = "elmah";
    options.LogPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Elmah");
    options.OnPermissionCheck = context =>
        builder.Environment.IsDevelopment() ||
        (context.User.Identity?.IsAuthenticated ?? false);
});

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
builder.Services.AddHttpContextAccessor();
builder.Services.AddCarGalaryDependencies();

// =======================
// Build App
// =======================
var app = builder.Build();

// =======================
// Middleware Pipeline
// =======================

// 1. Global exception handling (FIRST)
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. HTTPS
app.UseHttpsRedirection();

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
app.UseElmah();
// 6. Endpoints
app.MapControllers();




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
