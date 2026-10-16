using Microsoft.EntityFrameworkCore;
using RpaAgent.Domain;
namespace RpaAgent.Infrastructure;
// 统一限制租户业务表访问；应用服务必须额外校验 Company 授权范围。
public static class TenantIsolation
{
    public static IQueryable<T> ForTenant<T>(this IQueryable<T> query, Guid tenantId)
        where T : TenantEntity
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId required", nameof(tenantId));
        return query.Where(x => x.TenantId == tenantId);
    }
}