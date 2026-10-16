# REST API 规范 v1
根路径 /api/v1，OAuth2/OIDC JWT，租户来自已验证身份，禁止信任请求 tenantId。
实体资源：/tenants、/companies、/tenant-companies、/government-systems、/system-accounts、/auth-profiles、/data-sources、/business-operations、/business-requests、/business-records、/routing-rules、/workflows、/workers、/usb-devices、/audit-logs。
关键操作：
- POST /routing/resolve 业务路由预演
- POST /business-requests/{id}/validate|submit|cancel|retry|reconcile
- POST /workflows/{id}/validate|simulate|publish
- POST /executions/{id}/pause|resume|cancel|retry
- GET /executions/{id}/events|artifacts
- POST /worker-tasks/claim|heartbeat|complete
- POST /auth/challenge|respond|cancel
- POST /usb-devices/lease|release
所有创建/提交使用 Idempotency-Key；分页 page/pageSize（最大 200）；异步操作返回 202 + taskId；错误体 {code,message,traceId,details}。
HTTP 401/403/404/409/422/429/500；状态变更使用 If-Match；SignalR /hubs/executions 的订阅权限由服务端验证。
本文件为 API 路由及规范设计，完整 DTO 与逐字段 JSON Schema 在后续实现前需补全。