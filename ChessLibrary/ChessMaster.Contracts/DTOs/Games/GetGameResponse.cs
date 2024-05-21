namespace ChessMaster.Contracts.DTOs.Games;

public record GetGameResponse(
    Guid Id,
    Guid CreatorUserId,
    DateTime CreationTime,
    string Fen,
    Enum GameState,
    Guid? WhitePlayerId,
    Guid? BlackPlayerId,
    Guid? WinnerId
    );