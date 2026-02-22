using System.Text;
using CarGalary.Api;
using CarGalary.Application.Validations;
using CarGalary.Domain.Entities;
using CarGalary.Infrastructure.Auth;
using CarGalary.Infrastructure.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

// Authorization & Authentication
builder.Services.AddAuthorization();


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
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
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

// =======================
// Middleware Pipeline
// =======================

// 1. Global exception handling (FIRST)
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. HTTPS
app.UseHttpsRedirection();

// 3. Routing
app.UseRouting();




// Swagger (DEV only)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Test endpoint
app.MapGet("/api/version", () => Results.Ok(new { vers = "1.0" }));
// 6. Endpoints
app.MapControllers();
// 4. Authentication / Authorization
app.UseAuthentication();
app.UseAuthorization();




app.Run();
