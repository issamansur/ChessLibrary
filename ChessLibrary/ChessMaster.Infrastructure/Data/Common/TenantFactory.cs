using ChessMaster.Application.Common;
using ChessMaster.Infrastructure.Data.Repositories;

namespace ChessMaster.Infrastructure.Data.Common;

public class TenantFactory: ITenantFactory
{
    private readonly Lazy<ITenantRepository> _tenantRepository;
    
    public TenantFactory(ChessMasterDbContext dbContext)
    {
        _tenantRepository = new Lazy<ITenantRepository>(() => new TenantRepository(dbContext));
    }
    
    public ITenantRepository GetRepository()
    {
        return _tenantRepository.Value;
    }
}