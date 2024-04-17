namespace ChessMaster.Application.Users;

public class CreateUserCommand: IRequest<User>
{
    public string Username { get; }

    public CreateUserCommand(string username)
    {
        Username = username;
    }
}