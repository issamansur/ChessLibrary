namespace ChessMaster.Contracts.DTOs.Accounts;

public record RegisterAccountResponse(string Token, Guid UserId, string Username);