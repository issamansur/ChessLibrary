namespace ChessMaster.Application.Accounts;

public class RegisterAccountCommand : IRequest<Unit>
{
    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public RegisterAccountCommand(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}