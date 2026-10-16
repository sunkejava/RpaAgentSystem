using RpaAgent.Domain;
using Xunit;
namespace RpaAgent.Domain.Tests;
public class BusinessRouterTests
{
    [Fact]
    public void RejectsUnauthorisedBusiness()
    {
        var req = new RoutingRequest(Guid.NewGuid(),Guid.NewGuid(),"A","SI_EMPLOYEE_ADD");
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new BusinessRouter().Resolve(req, [], [], [], []));
        Assert.Equal("BUSINESS_NOT_AUTHORIZED",ex.Message);
    }
    [Fact]
    public void ResolvesExactlyOneAuthorizedAccount()
    {
        var tenant = Guid.NewGuid(); var company = Guid.NewGuid(); var sys = new GovernmentSystem{Code="SI-A",Name="Test",RegionCode="A",BusinessDomain="SI"};
        var account = new SystemAccount{TenantId=tenant,CompanyId=company,GovernmentSystemId=sys.Id,AccountLabel="test",CredentialReference="vault:example",AuthMethod="PasswordCaptcha"};
        var op = new SystemOperation{GovernmentSystemId=sys.Id,OperationCode="SI_EMPLOYEE_ADD",WorkflowCode="SI_ADD_A"};
        var grant = new BusinessAuthorization{TenantId=tenant,CompanyId=company,OperationCode=op.OperationCode};
        var result = new BusinessRouter().Resolve(new(tenant,company,"A",op.OperationCode),[sys],[op],[account],[grant]);
        Assert.Equal(sys.Id,result.SystemId);
        Assert.Equal(account.Id,result.AccountId);
    }
}