using Akka.Actor;

namespace ChessMaster.Infrastructure.Actors.AkkaNet.Common;

public abstract class ActorWithTenant: UntypedActor
{
    private ITenantFactory TenantFactory { get; init; }
    
    public ActorWithTenant(ITenantFactory tenantFactory) : base()
    {
        TenantFactory = tenantFactory;
    }
    
    protected ITenantRepository GetTenant()
    {
        return TenantFactory.GetRepository();
    }
}