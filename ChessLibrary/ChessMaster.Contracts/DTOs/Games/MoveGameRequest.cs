namespace ChessMaster.Contracts.DTOs.Games;

public record MoveGameRequest(Guid GameId, string Move);