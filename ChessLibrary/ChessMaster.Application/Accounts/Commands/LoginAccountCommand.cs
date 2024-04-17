namespace ChessMaster.Application.Accounts.Commands;

public class LoginAccountCommand: IRequest
{
    public string Login { get; }
    public string Password { get; }
    
    public LoginAccountCommand(string login, string password)
    {
        Login = login;
        Password = password;
        
        var validator = new LoginAccountCommandValidator();
        validator.ValidateAndThrow(this);
    }
}