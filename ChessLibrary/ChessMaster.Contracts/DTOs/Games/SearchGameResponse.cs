namespace ChessMaster.Contracts.DTOs.Games;

public record SearchGameResponse(IReadOnlyCollection<GetGameResponse> Games);