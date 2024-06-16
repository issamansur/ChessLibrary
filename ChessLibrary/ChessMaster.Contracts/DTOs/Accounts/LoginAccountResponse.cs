namespace ChessMaster.Contracts.DTOs.Accounts;

public record LoginAccountResponse(string Token, Guid UserId, string Username);