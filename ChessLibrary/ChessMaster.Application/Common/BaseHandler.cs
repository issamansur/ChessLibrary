namespace ChessMaster.Application.Common;

public abstract class BaseHandler
{
    private ITenantFactory TenantFactory { get; init; }
    
    protected BaseHandler(ITenantFactory tenantFactory)
    {
        TenantFactory = tenantFactory;
    }
    
    protected ITenantRepository GetTenant()
    {
        return TenantFactory.GetRepository();
    }
}