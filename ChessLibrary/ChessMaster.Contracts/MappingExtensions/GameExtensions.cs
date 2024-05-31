using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Application.CQRS.Games.Filters;
using ChessMaster.Application.CQRS.Games.Queries;
using ChessMaster.Contracts.DTOs.Games;

namespace ChessMaster.Contracts.MappingExtensions;

public static class GameExtensions
{
    public static CreateGameCommand ToCommand(this CreateGameRequest request, Guid playerId)
    {
        return new CreateGameCommand(playerId);
    }
    
    public static CreateGameResponse ToCreateResponse(this Game game)
    {
        return new CreateGameResponse(game.Id);
    }
    
    public static JoinGameCommand ToCommand(this JoinGameRequest request, Guid playerId)
    {
        return new JoinGameCommand(request.GameId, playerId);
    }

    public static JoinGameResponse ToJoinResponse(this Game game)
    {
        return new JoinGameResponse(game.Id);
    }

    public static MoveGameCommand ToCommand(this MoveGameRequest request, Guid playerId)
    {
        return new MoveGameCommand(request.GameId, playerId, request.Move);
    }
    
    public static MoveGameResponse ToMoveResponse(this Game game)
    {
        return new MoveGameResponse(
            game.Id,
            game.CreatorUserId,
            game.CreationTime,
            game.Fen,
            game.GameState,
            game.WhitePlayerId,
            game.BlackPlayerId,
            game.WinnerId
        );
    }
    
    public static GetGameQuery ToQuery(this GetGameRequest request)
    {
        return new GetGameQuery(request.GameId);
    }
    
    public static GetGameResponse ToGetResponse(this Game game)
    {
        return new GetGameResponse(
            game.Id, 
            game.CreatorUserId, 
            game.CreationTime, 
            game.Fen, 
            game.GameState, 
            game.WhitePlayerId, 
            game.BlackPlayerId, 
            game.WinnerId
        );
    }
    
    public static SearchGameQuery ToQuery(this SearchGameRequest request)
    {
        var filter = new GameFilter(request.PlayerId, request.State, request.Page, request.PageSize);
        
        return new SearchGameQuery(filter);
    }
    
    public static SearchGameResponse ToSearchResponse(this IReadOnlyCollection<Game> games)
    {
        var collection = games.Select(g => g.ToGetResponse()).ToArray();
        
        return new SearchGameResponse(collection);
    }
}