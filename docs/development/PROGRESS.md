# RpaAgentSystem 研发进度（2026-10-17）

## 已提交
- [x] 正式需求规格与架构设计 Markdown 仓库版：docs/requirements/
- [x] ER / API / Workflow / UI 设计初版
- [x] .NET 10 Domain / Infrastructure / API 项目骨架
- [x] PostgreSQL DbContext 实体、索引与初始 SQL bootstrap 参考
- [x] ClaimsTenantContext、ForTenant 查询扩展（尚未接入 JWT）
- [x] 确定性 BusinessRouter 与 xUnit 测试源码
- [x] Vue 3 / Element Plus 前端骨架及静态原型
- [x] GitHub Actions 构建配置

## 未完成与发布门禁
- [ ] 原始排版 DOCX 二进制文件上传仓库（当前已上传 Markdown 版本）
- [ ] CI 实际构建结果核验与错误修复
- [ ] EF Core 正式 Migration（当前 SQL bootstrap 不能替代）
- [ ] PostgreSQL RLS 与租户隔离端到端测试
- [ ] JWT/OIDC 验签、用户/租户/企业授权
- [ ] 强类型 OpenAPI DTO 与实际 CRUD API
- [ ] Vue 页面与真实 API 联调
- [ ] Workflow Runtime / Worker / UKey / 认证协作
- [ ] 单地区社保业务端到端验收

当前不能用于生产：尚未完成身份认证、RLS、敏感数据保护与真实业务审批。
