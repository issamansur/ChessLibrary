namespace ChessMaster.Application.Common;

public abstract class Handler<T>
{
    private ITenantRepository TenantRepository { get; init; }
    
    public Handler(ITenantRepository tenantRepository)
    {
        TenantRepository = tenantRepository;
    }

    public abstract Task Handle(IRequest<T> request, CancellationToken cancellationToken = default);
}