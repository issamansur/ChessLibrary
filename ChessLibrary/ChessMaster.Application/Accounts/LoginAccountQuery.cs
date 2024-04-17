namespace ChessMaster.Application.Accounts;

public class LoginAccountQuery: IRequest<Unit>
{
    public string Login { get; }
    public string Password { get; }
    
    public LoginAccountQuery(string login, string password)
    {
        Login = login;
        Password = password;
    }
}