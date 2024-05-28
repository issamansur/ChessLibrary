namespace ChessMaster.Application.Services;

public interface IJwtService
{
    string GenerateToken(Account account);
}