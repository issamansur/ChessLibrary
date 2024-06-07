using ChessMaster.Infrastructure.Data.Common;
using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;

public class GrainWithTenant: Grain
{
    private ITenantFactory TenantFactory { get; init; }
    
    public GrainWithTenant(ITenantFactory tenantFactory) : base()
    {
        TenantFactory = tenantFactory;
    }
    
    protected ITenantRepository GetTenant()
    {
        return TenantFactory.GetRepository();
    }
}