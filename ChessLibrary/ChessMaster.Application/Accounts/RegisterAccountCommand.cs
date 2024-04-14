namespace ChessMaster.Application.Accounts;

public class CreateAccountCommand : IRequest<Account>
{
    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public CreateAccountCommand(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}