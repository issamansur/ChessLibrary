using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure.Actors.AkkaNet.Common;

public abstract class MyUntypedActor: UntypedActor
{
    protected readonly IServiceScopeFactory _serviceScopeFactory;

    protected MyUntypedActor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
}