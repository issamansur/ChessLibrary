namespace ChessMaster.Infrastructure.Actors.Common;

public record MoveGameMessage(Guid GameId, string Move);