namespace ChessMaster.Application.CQRS.Games.Commands;

public class CreateGameCommand: IRequest<Game>
{
    public Guid CreatorUserId { get; }
    
    public CreateGameCommand(Guid creatorUserId)
    {
        CreatorUserId = creatorUserId;
    }
}