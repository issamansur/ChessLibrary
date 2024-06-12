using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;
using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans;

public class MicrosoftOrleansSystem
{
    IGrainFactory GrainFactory { get; init; }
    
    public MicrosoftOrleansSystem(IGrainFactory grainFactory)
    {
        GrainFactory = grainFactory;
    }
}