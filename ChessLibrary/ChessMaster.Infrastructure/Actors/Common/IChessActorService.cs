using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Application.CQRS.Games.Queries;

namespace ChessMaster.Infrastructure.Actors.Common;

public interface IChessActorService
{
    Task<Game> GetGameAsync(GetGameQuery getGameQuery, CancellationToken cancellationToken);
    Task<Game> JoinGameAsync(JoinGameCommand joinGameCommand, CancellationToken cancellationToken);
    Task<Game> MoveGameAsync(MoveGameCommand moveGameCommand, CancellationToken cancellationToken);
}