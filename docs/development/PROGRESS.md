# 研发进度

- [x] 需求设计、ER 草案、API 路由草案、Workflow 协议、静态原型
- [x] .NET 10 Domain/Infrastructure/API 基础工程文件
- [x] PostgreSQL EF Core 实体与索引映射初版
- [ ] 在具有 .NET 10 SDK 的 CI 环境编译验证
- [ ] EF Core 初始 Migration 生成与实际 PostgreSQL 验证
- [ ] JWT 租户上下文与授权策略
- [ ] 字段级 OpenAPI DTO 与实际 CRUD API
- [ ] Vue 3 真实工程及后端联调
- [ ] Workflow Runtime / Worker / 业务路由端到端测试

当前代码是安全的初始骨架，不具备真实政务申报能力。尚未执行 dotnet build（当前执行环境没有 .NET SDK）。
