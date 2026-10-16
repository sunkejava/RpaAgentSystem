using Microsoft.EntityFrameworkCore;
using RpaAgent.Domain;
using RpaAgent.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddDbContext<RpaDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Default") ?? "Host=localhost;Database=rpaagent;Username=rpaagent;Password=change-me"));
var app = builder.Build();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
// 开发阶段仅开放只读健康检查。写接口必须先接入 JWT、租户上下文和授权策略。
app.Run();
