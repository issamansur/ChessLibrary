namespace ChessMaster.Application.CQRS.Users.Queries;

public class GetUserByUsernameQuery: IRequest<User>
{
    public string Username { get; set; }
    
    public GetUserByUsernameQuery(string username)
    {
        Username = username;
    }
}