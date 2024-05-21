namespace ChessMaster.Contracts.DTOs.Accounts;

public record RegisterAccountRequest(
    string Username, 
    string Email, 
    string Password
);