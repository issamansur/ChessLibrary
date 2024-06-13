using ChessMaster.Application.CQRS.Games.Commands;

namespace ChessMaster.Infrastructure.Actors.Common;

public interface IChessActorService
{
    Task<Game> JoinGameAsync(JoinGameCommand joinGameCommand, CancellationToken cancellationToken);
    Task<Game> MoveGameAsync(MoveGameCommand moveGameCommand, CancellationToken cancellationToken);
}