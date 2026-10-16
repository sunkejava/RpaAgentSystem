# Workflow DSL 与 Worker 协议 v1
DSL JSON：dslVersion=1.0，code，variables，nodes，edges。节点类型 start/end/tool/condition/foreach/parallel/subflow/agent/wait-human/delay/transform/try-catch。
每节点：id,type,inputs,timeoutMs,retryPolicy,idempotency,approvalPolicy,onError；Tool 参数使用 JSON Schema 校验。
状态：Workflow Created→Validated→Published→Deprecated；Execution Queued→Leased→Running→WaitingHuman/Paused/Unknown/Succeeded/Failed/Cancelled。
Unknown 状态必须核验政务系统回执，不自动重复提交。
Worker 注册 {workerId,platform,capabilities,tenantScope,version}；心跳 {workerId,load,activeLeaseIds}；领取 {taskId,executionId,leaseToken,expiresAt,workflowVersionId,inputsRef}；事件 {executionId,sequence,eventType,redactedPayload,occurredAt}；完成 {executionId,leaseToken,state,artifacts,verification}。
租约采用 fencing token；事件序列幂等去重，重连续传；持久化事件后广播 SignalR。
OTP/扫码凭据不得进入 LLM；UKey 私钥不导出；提交、签名、删除等高风险操作遵循审批策略。