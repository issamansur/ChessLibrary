namespace ChessMaster.Contracts.DTOs.Users;

public record SearchUserResponse(
    IReadOnlyCollection<GetUserResponse> Users
);