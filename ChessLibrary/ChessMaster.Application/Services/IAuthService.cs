namespace ChessMaster.Application.Services;

public interface IAuthService
{
    string GenerateToken(Account account);
}