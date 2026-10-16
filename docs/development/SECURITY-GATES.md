# 安全门禁与当前限制
1. 未实现 JWT/OIDC 验签前，所有真实企业数据的 CRUD、路由、任务提交 API 必须保持关闭。
2. 仅在生产验证的 TenantContext 中取 TenantId，禁止请求体/请求头任意指定租户。
3. 应用层 ForTenant 只是第一层隔离，生产 PostgreSQL 还必须配置并测试 RLS、复合外键和跨租户拒绝。
4. 初始 SQL 是数据库 bootstrap 参考，不等同于 EF Core Migration，不能与 EF 自动建表混用。
5. 示例数据库密码仅用于本地开发；上线前改用 Secret Manager / Vault。
6. 尚未执行 dotnet build、dotnet test 或 PostgreSQL 迁移验证；需由 CI 反馈后再确认。
7. 任何真实申报提交必须经授权、审批、幂等检查与官方结果核验。
