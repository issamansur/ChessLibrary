namespace ChessMaster.Application.Common;

public abstract class BaseHandler
{
    private ITenantFactory TenantFactory { get; init; }
    protected ITenantRepository TenantRepository => TenantFactory.GetRepository();
    
    protected BaseHandler(ITenantFactory tenantFactory)
    {
        TenantFactory = tenantFactory;
    }
}