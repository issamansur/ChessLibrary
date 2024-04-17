namespace ChessMaster.Application.Games;

public class CreateGameCommand: IRequest<Game>
{
    public Guid CreatorUserId { get; }
    
    public CreateGameCommand(Guid creatorUserId)
    {
        CreatorUserId = creatorUserId;
    }
}