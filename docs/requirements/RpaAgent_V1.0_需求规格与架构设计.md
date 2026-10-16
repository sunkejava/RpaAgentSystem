# RpaAgent V1.0 产品需求规格与总体架构设计
> 版本：1.0 设计基线草案 · 日期：2026-10-17 · 适用：产品、业务、架构、研发、测试、运维和安全评审

## 1. 背景与目标
面向人力资源服务机构，为多个委托企业在社保、医保、公积金和劳动用工备案系统完成申报、查询、下载与回执归档。各地区平台、登录身份、账号、验证码、短信、二维码和 UKey 差异较大。
目标：按租户、企业、办理事项和地区确定授权系统、账号、认证策略、数据映射、工作流和 Worker；提供批量处理、人工认证协作、可审计结果与异常恢复。未来演进为独立企业 SaaS。
非目标：绕过验证码/短信/扫码/数字证书安全控制；无授权访问政务系统；在未经确认情况下自动重复申报。

## 2. 用户与权限
平台管理员、代理机构管理员、企业管理员、业务主管、业务经办、审核人员、审计人员、运维人员。
租户 Tenant 是数据隔离边界；Company 是法定业务主体；TenantCompany 表示代理服务关系；同一企业可关联多个租户但账号、凭据、授权、任务及数据严格按租户隔离。
权限至少覆盖菜单、API、企业、业务操作、账号、Worker、文件和审批。业务操作应按 CompanyBusinessAuthorization 与有效期校验。

## 3. 业务领域
业务分类：SI 社保、MI 医保、HF 公积金、LABOR 劳动用工备案。
标准操作代码示例：SI_EMPLOYEE_ADD、SI_EMPLOYEE_REMOVE、SI_PAYMENT_QUERY、MI_EMPLOYEE_ADD、MI_PAYMENT_DOWNLOAD、HF_EMPLOYEE_ADD、HF_CONTRIBUTION_QUERY、LABOR_CONTRACT_REGISTER。
政府系统注册表记录：地区代码、业务领域、系统入口、登录方式、支持操作、系统版本、运行环境要求。
企业系统账号允许一企多账号、一账号多操作授权、授权经办人、优先级和失效时间；原始凭据放独立密钥库，仅保留引用。

## 4. 端到端业务流程
选择企业与业务 → 选择 Excel/ERP API/手工数据 → 标准化字段与校验 → 企业授权检查 → 路由预演 → 人工确认高风险动作 → 申请 Worker 与 UKey 租约 → 身份认证 → 自动执行 → 读取政务平台确认结果 → 回执归档 → 结果报告。
跨系统综合办理使用 BusinessProcess → BusinessTask → WorkflowExecution → ToolExecution 分层。部分成功必须单独记录，不自动撤销其他系统成功事项。

## 5. 功能需求
FR-01 租户、用户、组织与 RBAC；FR-02 企业档案、联系人、委托协议与授权；FR-03 政务系统、地区与操作注册；FR-04 系统账号、凭据引用与认证策略；FR-05 Excel/ERP/手工数据源、模板与地区字段映射；FR-06 确定性业务路由与冲突处理；FR-07 工作流定义、版本、测试、发布、回滚；FR-08 浏览器和 Windows 桌面自动化；FR-09 密码、图形验证码、短信、扫码、UKey 人工协作；FR-10 Windows/Linux/VM Worker 调度、心跳、租约与资源绑定；FR-11 批量任务、幂等、取消、暂停、恢复、补偿与回执核验；FR-12 审计、截图、视频、结构化日志和失败诊断；FR-13 Agent 辅助编排、异常分析与经审批修复；FR-14 报表、结果导出与任务通知。
V1.0 必做：FR-02 至 FR-12 的首地区社保端到端闭环与基本权限。Agent 自主修复、全国多地区和 SaaS 计费作为后续版本。

## 6. 认证状态机
Pending → CheckingSession → EnteringCredentials → WaitingCaptcha / WaitingSms / WaitingQrScan / WaitingUKey → Verifying → Authenticated；终态 Failed / Expired / Cancelled。
认证挑战通过 AuthChallengeId 与有权限的人员绑定；短信一次性使用且短时过期；二维码需授权人员扫码；证书私钥不导出；敏感内容不写日志、不发给 LLM。

## 7. 路由规则
输入 TenantId、CompanyId、RegionCode、OperationCode、DataSourceId；输出 SystemId、AccountBindingId、AuthProfileId、WorkflowVersionId、RequiredCapabilities、WorkerAffinity。
先验证委托授权与业务操作；公司专属规则优先于地区规则、再优先于系统默认规则。无路由则拒绝，多路由无确定优先级则返回冲突，不允许 AI 猜测。
路由结果只保存凭据引用，不携带密码。

## 8. 工作流与 Agent
Workflow DSL JSON 包含 dslVersion、code、variables、nodes、edges、timeout、retryPolicy、approvalPolicy、idempotency。
节点类型：start/end/tool/condition/foreach/parallel/subflow/agent/wait-human/transform/try-catch。
Agent 只对允许的工具集调用；业务提交、签名、删除等动作需权限与审批；已提交但结果未知时先对账核验，不自动重试。
发布的工作流版本不可变，执行绑定版本哈希。

## 9. Worker 与 UKey
调度依据 OS、浏览器、Windows 桌面能力、证书、USB 设备、地区、租户隔离、负载和亲和性。Windows Service 与交互式 Desktop Agent 分离，避免 Session 0 问题。
UKey 设备以 DeviceLease 独占使用；心跳续约、fencing token 防止过期任务继续操作；签名完成并确认会话不再需要设备后才能释放。USB 重连与设备枚举异常需要完整链路日志。

## 10. 数据模型
Tenant、User、TenantMember、Company、TenantCompany、BusinessAuthorization、GovernmentSystem、SystemOperation、SystemAccount、CredentialReference、AuthProfile、AuthSession、AuthChallenge、DataSource、DataMapping、BusinessRequest、BusinessRecord、BusinessTask、RoutingRule、RoutingDecision、Workflow、WorkflowVersion、WorkflowExecution、Worker、WorkerCapability、TaskLease、UsbDevice、DeviceLease、ExecutionEvent、Artifact、AuditLog。
PostgreSQL 正式环境；EF Core 迁移；所有业务数据有 TenantId、UTC 时间戳和必要索引；数据库 RLS 作为纵深防御。详见 docs/database/ER.md。

## 11. API 设计
/api/v1 版本化 REST；OIDC/JWT；请求关联 TraceId；统一错误 {code,message,traceId,details}；分页、条件查询、幂等键、If-Match；异步操作返回 202 与任务标识。
接口覆盖企业、系统、账号、认证、数据源、路由、业务任务、工作流、Worker、设备与审计。详见 docs/api/openapi.json。

## 12. 前端设计
Vue 3 + TypeScript + Element Plus；总览、企业档案、系统账号、政务系统、业务办理、批量任务、认证协作、执行监控、工作流设计器、Worker/UKey 资源、审计日志、系统设置。
每页需具备权限校验、加载/空状态、查询分页、批量操作反馈、危险操作确认与错误提示。原型参见 frontend/prototype/index.html。

## 13. 非功能与安全
敏感个人数据加密与最小化保留；租户和企业授权隔离；凭据密钥库；端到端 TLS；日志脱敏；数据访问与导出审计；数据保留期限可配置；支持按客户部署。
运行指标需在首个真实地区业务试点中量测确认，不凭空承诺成功率或处理时延。对异常恢复与重试设置可配置上限；审计记录与结果回执不可被普通业务用户篡改。

## 14. 测试与验收
验收覆盖：企业创建与授权；多账号匹配；无授权拒绝；路由冲突拒绝；Excel 校验；短信/扫码人工认证；UKey 租约冲突；Worker 离线与恢复；批量部分成功；提交结果未知时对账；真实回执归档；跨租户越权拒绝。
V1.0 成功条件：单地区社保增员/减员/查询从企业选择、数据导入、登录、自动执行到真实平台回执全链路可复现且可审计。

## 15. 实施计划
P0 需求冻结与领域模型；P1 .NET 10、PostgreSQL、租户安全与 API；P2 Vue 3 企业/账号/数据源；P3 工作流/Worker/认证与 UKey；P4 单地区社保端到端试点；P5 稳定性、审计、运维和验收。
后续 V1.5 引入 AI 辅助工作流与跨系统编排；V2.0 增加 SaaS 自助租户、配额、计费和企业本地 Worker。

## 16. 设计决策与待确认
优先服务人力资源公司，未来独立企业 SaaS；采用多租户逻辑模型、单租户部署起步；业务路由确定性；提交与签名需审批策略；原始凭据不入数据库。
待业务确认：首个地区及政务系统、企业授权材料要求、具体业务模板、UKey 设备型号、审批分级、数据保留期限、SLA 与目标规模。

> 本文件为正式需求规格与架构设计的 Markdown 仓库版。原始排版 Word 文件仍需以二进制方式上传。