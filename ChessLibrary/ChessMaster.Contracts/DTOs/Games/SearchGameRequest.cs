using ChessMaster.Domain.States;

namespace ChessMaster.Contracts.DTOs.Games;

public record SearchGameRequest(
    Guid? PlayerId,
    State? State,
    int Page,
    int PageSize = 10
);
    