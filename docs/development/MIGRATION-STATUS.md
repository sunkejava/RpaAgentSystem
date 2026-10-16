# 数据库迁移与租户隔离实施状态

## 已提交
- EF Core Migration 类：`Migrations/202610170001_InitialSchema.cs`
- SQL 嵌入资源：`backend/src/Directory.Build.targets`
- 初始 Schema：`backend/database/001_initial.sql`
- 租户成员与 RLS：`backend/database/002_tenant_members.sql`、`003_tenant_rls.sql`
- 设计时工厂：`RpaDbContextFactory`（环境变量 `RPA_MIGRATION_CONNECTION`）
- PostgreSQL RLS 回归脚本：`backend/tests/sql/tenant_rls.sql`
- CI：`.github/workflows/database-ci.yml`（.NET 10 编译、dotnet ef database update、PostgreSQL 17 测试）

## 尚未验收
- 本执行环境没有 .NET SDK，不能在本地执行 dotnet build、dotnet ef 或 dotnet test。
- GitHub 当前提交未返回可确认的状态检查结果；不宣称构建/迁移/测试成功。
- 当前初始 Migration 使用 SQL 嵌入资源，尚缺 EF Core ModelSnapshot；后续模型变更前需补齐并用 dotnet ef migrations has-pending-model-changes 验证。
- RLS 只保护带 TenantId 的租户业务表；全局 Companies、GovernmentSystems 必须经受权绑定查询。
- `TenantAccess.VerifyMemberAsync` 仅提供数据库成员校验方法，尚未集成至可用 API，且须在数据库事务内调用。
- 企业管理 API 仍保持关闭；等待 JWT、RLS、CI 集成测试通过再启用。

## 操作步骤
1. 配置 PostgreSQL 17 测试实例和 `RPA_MIGRATION_CONNECTION`。
2. 运行 `dotnet build backend/src/RpaAgent.Api/RpaAgent.Api.csproj -c Release`。
3. 安装 `dotnet-ef` 10.x；运行 `dotnet ef database update --project backend/src/RpaAgent.Infrastructure --startup-project backend/src/RpaAgent.Api`。
4. 使用非超级用户执行租户隔离回归测试，确认无上下文返回 0 行、事务局部上下文仅可见本租户。
5. 核验 GitHub Actions 通过后，再启用企业 CRUD。

**注意：** 生产环境应用账号不可使用 superuser/BYPASSRLS，连接池中必须以事务局部方式设置 tenant_id，提交事务后自动清除。