using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RpaAgent.Infrastructure;

/// <summary>dotnet ef 设计时工厂；仅从环境变量读取连接字符串。</summary>
public sealed class RpaDbContextFactory : IDesignTimeDbContextFactory<RpaDbContext>
{
    public RpaDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("RPA_MIGRATION_CONNECTION")
            ?? throw new InvalidOperationException("Set RPA_MIGRATION_CONNECTION before running migrations.");
        var options = new DbContextOptionsBuilder<RpaDbContext>()
            .UseNpgsql(connectionString).Options;
        return new RpaDbContext(options);
    }
}
