using ChessMaster.Domain.States;

namespace ChessMaster.Contracts.DTOs.Games;

public record MoveGameResponse(
    Guid Id,
    Guid CreatorUserId,
    DateTime CreationTime,
    string Fen,
    State GameState,
    Guid? WhitePlayerId,
    Guid? BlackPlayerId,
    Guid? WinnerId
);