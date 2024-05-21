namespace ChessMaster.Contracts.DTOs.Games;

public record JoinGameRequest(Guid GameId, Guid PlayerId);