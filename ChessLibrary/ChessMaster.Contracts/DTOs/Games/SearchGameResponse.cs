namespace ChessMaster.Contracts.DTOs.Games;

public record SearchGameResponse(IEnumerable<GetGameResponse> Games);