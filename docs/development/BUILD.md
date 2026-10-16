# 构建与开发验证

前提：.NET 10 SDK、Node.js 22、PostgreSQL 17。

```bash
docker compose -f deploy/docker-compose.yml up -d
dotnet restore backend/src/RpaAgent.Api/RpaAgent.Api.csproj
dotnet build backend/src/RpaAgent.Api/RpaAgent.Api.csproj -c Release
dotnet test backend/tests/RpaAgent.Domain.Tests/RpaAgent.Domain.Tests.csproj -c Release
cd frontend && npm install && npm run build
```

生产部署必须更换示例数据库密码、配置密钥管理、JWT 鉴权、数据库 RLS 和实际迁移；当前版本不可用于真实企业敏感数据或政务申报。
