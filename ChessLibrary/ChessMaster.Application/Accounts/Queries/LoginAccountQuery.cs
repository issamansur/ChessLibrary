namespace ChessMaster.Application.Accounts.Queries;

public class LoginAccountQuery: IRequest
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