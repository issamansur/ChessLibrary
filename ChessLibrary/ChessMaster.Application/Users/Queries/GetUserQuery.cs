namespace ChessMaster.Application.Users.Queries;

public class GetUserQuery: IRequest<User>
{
    public Guid Id { get; set; }
    
    public GetUserQuery(Guid id)
    {
        Id = id;
    }
}