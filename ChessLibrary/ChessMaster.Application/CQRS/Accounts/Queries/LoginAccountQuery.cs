using ChessMaster.Application.DTOs;

namespace ChessMaster.Application.CQRS.Accounts.Queries;

public class LoginAccountQuery: IRequest<LoginResult>
{
    public string Login { get; }
    public string Password { get; }
    
    public LoginAccountQuery(string login, string password)
    {
        Login = login;
        Password = password;
        
        var validator = new LoginAccountQueryValidator();
        validator.ValidateAndThrow(this);
    }
}