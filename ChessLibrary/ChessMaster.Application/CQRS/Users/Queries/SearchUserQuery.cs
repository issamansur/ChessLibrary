namespace ChessMaster.Application.CQRS.Users.Queries;

public class SearchUserQuery: IRequest<IReadOnlyCollection<User>>
{
    public string Query { get; set; }
    
    public SearchUserQuery(string query)
    {
        Query = query;
    }
}