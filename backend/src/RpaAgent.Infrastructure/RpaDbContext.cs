using Microsoft.EntityFrameworkCore;
using RpaAgent.Domain;
namespace RpaAgent.Infrastructure;
public sealed class RpaDbContext(DbContextOptions<RpaDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<TenantCompany> TenantCompanies => Set<TenantCompany>();
    public DbSet<GovernmentSystem> GovernmentSystems => Set<GovernmentSystem>();
    public DbSet<SystemOperation> SystemOperations => Set<SystemOperation>();
    public DbSet<BusinessAuthorization> BusinessAuthorizations => Set<BusinessAuthorization>();
    public DbSet<SystemAccount> SystemAccounts => Set<SystemAccount>();
    public DbSet<BusinessRequest> BusinessRequests => Set<BusinessRequest>();
    public DbSet<BusinessRecord> BusinessRecords => Set<BusinessRecord>();
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();
    public DbSet<WorkflowExecution> WorkflowExecutions => Set<WorkflowExecution>();
    public DbSet<WorkerNode> Workers => Set<WorkerNode>();
    public DbSet<UsbDevice> UsbDevices => Set<UsbDevice>();
    public DbSet<DeviceLease> DeviceLeases => Set<DeviceLease>();
    public DbSet<ExecutionEvent> ExecutionEvents => Set<ExecutionEvent>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Tenant>().HasIndex(x => x.Name);
        b.Entity<Company>().HasIndex(x => x.CreditCode).IsUnique();
        b.Entity<TenantCompany>().HasIndex(x => new {x.TenantId,x.CompanyId}).IsUnique();
        b.Entity<GovernmentSystem>().HasIndex(x => x.Code).IsUnique();
        b.Entity<SystemOperation>().HasIndex(x => new {x.GovernmentSystemId,x.OperationCode}).IsUnique();
        b.Entity<BusinessAuthorization>().HasIndex(x => new {x.TenantId,x.CompanyId,x.OperationCode});
        b.Entity<SystemAccount>().HasIndex(x => new {x.TenantId,x.CompanyId,x.GovernmentSystemId});
        b.Entity<BusinessRequest>().HasIndex(x => new {x.TenantId,x.IdempotencyKey}).IsUnique();
        b.Entity<BusinessRecord>().HasIndex(x => new {x.TenantId,x.BusinessRequestId,x.SubjectKey}).IsUnique();
        b.Entity<WorkflowDefinition>().HasIndex(x => new {x.TenantId,x.Code,x.Version}).IsUnique();
        b.Entity<ExecutionEvent>().HasIndex(x => new {x.TenantId,x.WorkflowExecutionId,x.Sequence}).IsUnique();
        b.Entity<DeviceLease>().HasIndex(x => new {x.UsbDeviceId,x.Status});
    }
}