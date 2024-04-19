namespace ChessMaster.Application.Users.Queries;

public class SearchUserQuery: IRequest<User>
{
    public string Username { get; set; }
    
    public SearchUserQuery(string username)
    {
        Username = username;
    }
}