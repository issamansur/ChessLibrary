namespace ChessMaster.Contracts.DTOs.Users;

public record GetUserResponse(
    Guid Id,
    string Username
);