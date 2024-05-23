namespace ChessMaster.Contracts.DTOs.Users;

public record GetUserByUsernameResponse(
    Guid Id,
    string Username
);