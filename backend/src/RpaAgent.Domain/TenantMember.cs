namespace RpaAgent.Domain;

/// <summary>租户成员身份由受信任的 OIDC subject 与租户绑定，不能仅依赖 JWT 自报角色。</summary>
public sealed class TenantMember : TenantEntity
{
    public required string Subject { get; set; }
    public required string Role { get; set; }
    public bool Enabled { get; set; } = true;
}
