namespace ChessMaster.Contracts.DTOs.Accounts.Requests;

public record RegisterAccountRequest(string Username, string Email, string Password);