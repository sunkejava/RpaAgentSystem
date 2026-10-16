namespace RpaAgent.Domain;

// 业务路由必须确定性选择，禁止 Agent 猜测系统或账号。
public sealed record RoutingRequest(Guid TenantId, Guid CompanyId, string RegionCode, string OperationCode);
public sealed record RoutingDecision(Guid SystemId, Guid AccountId, string WorkflowCode);
public sealed class BusinessRouter
{
    public RoutingDecision Resolve(RoutingRequest request,
        IEnumerable<GovernmentSystem> systems,
        IEnumerable<SystemOperation> operations,
        IEnumerable<SystemAccount> accounts,
        IEnumerable<BusinessAuthorization> authorizations)
    {
        if (!authorizations.Any(x => x.TenantId == request.TenantId &&
            x.CompanyId == request.CompanyId && x.OperationCode == request.OperationCode &&
            (!x.ExpiresAt.HasValue || x.ExpiresAt > DateTimeOffset.UtcNow)))
            throw new InvalidOperationException("BUSINESS_NOT_AUTHORIZED");
        var candidates = (from system in systems
            join operation in operations on system.Id equals operation.GovernmentSystemId
            join account in accounts on system.Id equals account.GovernmentSystemId
            where system.RegionCode == request.RegionCode &&
                  operation.OperationCode == request.OperationCode &&
                  account.TenantId == request.TenantId &&
                  account.CompanyId == request.CompanyId && account.Enabled
            select new RoutingDecision(system.Id, account.Id, operation.WorkflowCode)).Distinct().ToArray();
        return candidates.Length switch
        {
            0 => throw new InvalidOperationException("ROUTE_NOT_FOUND"),
            1 => candidates[0],
            _ => throw new InvalidOperationException("ROUTE_AMBIGUOUS")
        };
    }
}