namespace ChessMaster.Application.Accounts.Commands;

public class RegisterAccountCommand : IRequest
{
    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public RegisterAccountCommand(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
        
        var validator = new RegisterAccountCommandValidator();
        validator.ValidateAndThrow(this);
    }
}