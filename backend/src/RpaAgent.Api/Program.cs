using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RpaAgent.Api;
using RpaAgent.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, ClaimsTenantContext>();
builder.Services.AddDbContext<RpaDbContext>(o => o.UseNpgsql(
    builder.Configuration.GetConnectionString("Default") ??
    throw new InvalidOperationException("ConnectionStrings:Default is required")));
var authority = builder.Configuration["Authentication:Authority"];
var audience = builder.Configuration["Authentication:Audience"];
if (string.IsNullOrWhiteSpace(authority) || string.IsNullOrWhiteSpace(audience))
    throw new InvalidOperationException("Authentication:Authority and Authentication:Audience are required");
if (!Uri.TryCreate(authority, UriKind.Absolute, out var issuerUri) || issuerUri.Scheme != Uri.UriSchemeHttps)
    throw new InvalidOperationException("Authentication:Authority must be an HTTPS issuer");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.Authority = authority;
    o.Audience = audience;
    o.RequireHttpsMetadata = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true,
        ValidateIssuerSigningKey = true, ClockSkew = TimeSpan.FromSeconds(30)
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("tenant-user", p => p.RequireAuthenticatedUser().RequireClaim("tenant_id"));
    options.AddPolicy("tenant-admin", p => p.RequireAuthenticatedUser().RequireClaim("tenant_id").RequireRole("tenant-admin"));
});
var app = builder.Build();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapCompanyEndpoints();
app.Run();
