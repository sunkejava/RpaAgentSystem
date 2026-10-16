# 身份认证配置
后端采用外部 OIDC 身份提供方（例如 Keycloak），由受信任的 HTTPS issuer 签发 JWT。
配置环境变量 Authentication__Authority 与 Authentication__Audience。令牌必须含 tenant_id（UUID）和角色 tenant-admin（仅创建企业需要）。
后端不会提供匿名注册、开发万能令牌或关闭签名校验的模式。
此阶段未完成用户/租户成员关系核验、RLS 和公司授权审查，不可接入真实客户数据。
数据库表需先通过受控迁移建立，不能在生产运行时自动 EnsureCreated。
