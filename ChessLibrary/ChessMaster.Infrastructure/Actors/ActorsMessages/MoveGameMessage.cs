namespace ChessMaster.Infrastructure.Actors.ActorsMessages;

public record MoveGameMessage(Guid GameId, string Move);