using System.Security.Claims;
namespace RpaAgent.Api;
// 租户上下文必须来自已验证 JWT 的声明，不能来自查询参数或请求头。
public interface ITenantContext { Guid TenantId { get; } }
public sealed class ClaimsTenantContext(IHttpContextAccessor accessor) : ITenantContext
{
    public Guid TenantId
    {
        get
        {
            var user = accessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("AUTH_REQUIRED");
            var claim = user.FindFirst("tenant_id")?.Value;
            if (!Guid.TryParse(claim, out var id) || id == Guid.Empty)
                throw new UnauthorizedAccessException("TENANT_CLAIM_REQUIRED");
            return id;
        }
    }
}