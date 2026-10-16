namespace RpaAgent.Domain;

// 所有租户数据通过 TenantId 进行隔离；真实生产还需启用数据库 RLS。
public abstract class TenantEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
public sealed class Tenant { public Guid Id { get; set; } = Guid.NewGuid(); public required string Name { get; set; } }
public sealed class Company { public Guid Id { get; set; } = Guid.NewGuid(); public required string Name { get; set; } public required string CreditCode { get; set; } public required string RegionCode { get; set; } }
public sealed class TenantCompany : TenantEntity { public Guid CompanyId { get; set; } public bool Enabled { get; set; } = true; }
public sealed class GovernmentSystem { public Guid Id { get; set; } = Guid.NewGuid(); public required string Code { get; set; } public required string Name { get; set; } public required string RegionCode { get; set; } public required string BusinessDomain { get; set; } }
public sealed class SystemOperation { public Guid Id { get; set; } = Guid.NewGuid(); public Guid GovernmentSystemId { get; set; } public required string OperationCode { get; set; } public required string WorkflowCode { get; set; } }
public sealed class BusinessAuthorization : TenantEntity { public Guid CompanyId { get; set; } public required string OperationCode { get; set; } public DateTimeOffset? ExpiresAt { get; set; } }
public sealed class SystemAccount : TenantEntity { public Guid CompanyId { get; set; } public Guid GovernmentSystemId { get; set; } public required string AccountLabel { get; set; } public required string CredentialReference { get; set; } public required string AuthMethod { get; set; } public bool Enabled { get; set; } = true; }
public sealed class BusinessRequest : TenantEntity { public Guid CompanyId { get; set; } public required string OperationCode { get; set; } public required string IdempotencyKey { get; set; } public string Status { get; set; } = "Pending"; public DateTimeOffset? SubmittedAt { get; set; } }
public sealed class BusinessRecord : TenantEntity { public Guid BusinessRequestId { get; set; } public required string SubjectKey { get; set; } public string Status { get; set; } = "Pending"; public string? ReceiptReference { get; set; } }
public sealed class WorkflowDefinition : TenantEntity { public required string Code { get; set; } public int Version { get; set; } public required string JsonDsl { get; set; } public bool Published { get; set; } }
public sealed class WorkflowExecution : TenantEntity { public Guid BusinessRequestId { get; set; } public Guid WorkflowDefinitionId { get; set; } public string Status { get; set; } = "Queued"; public Guid? WorkerId { get; set; } }
public sealed class WorkerNode : TenantEntity { public required string Name { get; set; } public required string Platform { get; set; } public string CapabilitiesJson { get; set; } = "[]"; public DateTimeOffset? LastHeartbeatAt { get; set; } }
public sealed class UsbDevice : TenantEntity { public required string DeviceKey { get; set; } public Guid? CompanyId { get; set; } public string Status { get; set; } = "Offline"; }
public sealed class DeviceLease : TenantEntity { public Guid UsbDeviceId { get; set; } public Guid WorkerId { get; set; } public long FencingToken { get; set; } public DateTimeOffset ExpiresAt { get; set; } public string Status { get; set; } = "Active"; }
public sealed class ExecutionEvent : TenantEntity { public Guid WorkflowExecutionId { get; set; } public long Sequence { get; set; } public required string EventType { get; set; } public string RedactedPayload { get; set; } = "{}"; }
