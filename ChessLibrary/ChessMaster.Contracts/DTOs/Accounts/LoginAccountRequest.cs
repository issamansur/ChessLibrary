namespace ChessMaster.Contracts.DTOs.Accounts;

public record LoginAccountRequest(
    string Login, 
    string Password
);