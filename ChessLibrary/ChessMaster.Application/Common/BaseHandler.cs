namespace ChessMaster.Application.Common;

public abstract class BaseHandler<T>
{
    private ITenantFactory TenantFactory { get; init; }
    protected ITenantRepository TenantRepository => TenantFactory.GetRepository();
    
    protected BaseHandler(ITenantFactory tenantFactory)
    {
        TenantFactory = tenantFactory;
    }

    protected void ValidateRequest(IRequest<T> request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
    }
}