namespace ChessMaster.Application.CQRS.Accounts.Queries;

public class LoginAccountQuery: IRequest<string>
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