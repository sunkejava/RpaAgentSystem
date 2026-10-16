using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaAgent.Infrastructure.Migrations;

/// <summary>初始 PostgreSQL Schema 与租户 RLS，按顺序执行仓库内嵌 SQL。</summary>
[DbContext(typeof(RpaDbContext))]
[Migration("202610170001_InitialSchema")]
public sealed class InitialSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var file in new[] { "001_initial.sql", "002_tenant_members.sql", "003_tenant_rls.sql" })
        {
            using var stream = typeof(InitialSchema).Assembly.GetManifestResourceStream("RpaAgent.Migrations." + file)
                ?? throw new InvalidOperationException("Missing embedded SQL migration: " + file);
            using var reader = new StreamReader(stream);
            var sql = reader.ReadToEnd();
            if (file == "001_initial.sql")
                sql = sql.Replace("BEGIN;", string.Empty).Replace("COMMIT;", string.Empty);
            migrationBuilder.Sql(sql);
        }
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        foreach (var name in new[] {
            "ExecutionEvents", "DeviceLeases", "UsbDevices", "WorkflowExecutions", "Workers",
            "WorkflowDefinitions", "BusinessRecords", "BusinessRequests", "SystemAccounts",
            "BusinessAuthorizations", "TenantCompanies", "SystemOperations", "GovernmentSystems",
            "Companies", "TenantMembers", "Tenants" })
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"" + name + "\" CASCADE;");
    }
}
