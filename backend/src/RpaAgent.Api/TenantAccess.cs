using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RpaAgent.Infrastructure;

namespace RpaAgent.Api;

/// <summary>请求级租户权限验证：先建立事务局部 RLS 上下文，再查询数据库成员资格。</summary>
public static class TenantAccess
{
    public static async Task<bool> VerifyMemberAsync(
        RpaDbContext db, ClaimsPrincipal principal, Guid tenantId,
        bool requireAdmin, CancellationToken cancellationToken)
    {
        if (tenantId == Guid.Empty || principal.Identity?.IsAuthenticated != true)
            return false;
        var subject = principal.FindFirst("sub")?.Value ??
                      principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(subject)) return false;
        // 调用者必须已经 BeginTransactionAsync。SET LOCAL 在事务结束时自动清除。
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT set_config('app.tenant_id', {tenantId.ToString()}, true)", cancellationToken);
        return await db.TenantMembers.AsNoTracking().AnyAsync(
            x => x.TenantId == tenantId && x.Subject == subject && x.Enabled &&
                 (!requireAdmin || x.Role == "tenant-admin"), cancellationToken);
    }
}
