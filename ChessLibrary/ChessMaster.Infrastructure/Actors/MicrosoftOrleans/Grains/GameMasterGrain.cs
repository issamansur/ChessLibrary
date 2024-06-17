using System.Threading.Channels;
using ChessMaster.ChessLibrary;
using ChessMaster.ChessLibrary.States;
using ChessMaster.Domain.States;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Grains;

public class GameMasterGrain: Grain, IGameMasterGrain
{
    public Guid GameId => this.GetPrimaryKey();
    public Game? Game { get; private set; }
    public Chess Chess { get; private set; }
    
    private bool _isFirstTime = true;
    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public GameMasterGrain(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    // Override methods
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"GameMasterGrain for game: {GameId} activating");
        if (_isFirstTime)
        {
            Console.WriteLine($"GameMasterGrain for game: {GameId} first time activation");
            await LoadGameState(cancellationToken);
            // Set flag to false
            _isFirstTime = false;
        }
        Console.WriteLine($"GameMasterGrain for game: {GameId} activated");
        await base.OnActivateAsync(cancellationToken);
    }
    
    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GameMasterGrain for game: {GameId} deactivating");
        await SaveGameState(cancellationToken);
        Console.WriteLine($"GameMasterGrain for game: {GameId} deactivated");
        await base.OnDeactivateAsync(reason, cancellationToken);
    }
    
    // Interface methods
    public async Task<Game> GetGameAsync(CancellationToken cancellationToken)
    {
        if (Game is null)
        {
            throw new InvalidOperationException("Game not found");
        }
        
        return Game;
    }

    public async Task<Game> JoinGameAsync(Guid playerId, CancellationToken cancellationToken)
    {
        if (Game is null)
        {
            throw new InvalidOperationException("Game not found");
        }
        
        try
        {
            Game!.Join(playerId);
            
            Console.WriteLine($"Player: {playerId} joined game: {GameId}");

            return Game;
        }
        catch (InvalidOperationException e)
        {
            //SendError(e.Message);
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
        var tenant = tenantFactory.GetRepository();
            
        await tenant.Games.UpdateAsync(Game!, cancellationToken);
        await tenant.CommitAsync(cancellationToken);

        return Game;
    }

    public async Task<Game> MoveGameAsync(
        Guid playerId,
        string move,
        CancellationToken cancellationToken
    )
    {
        if (Game is null)
        {
            throw new InvalidOperationException("Game not found");
        }

        if (Game!.GameState == State.Created)
        {
            SendError($"Game: {GameId} is not started yet", true);
        }
        
        if (playerId != Game!.WhitePlayerId && playerId != Game!.BlackPlayerId)
        {
            SendError($"Player: {playerId} is not in game: {GameId}");
        }
                
        if (playerId == Game!.WhitePlayerId && Chess.ActiveColor == Color.Black ||
            playerId == Game!.BlackPlayerId && Chess.ActiveColor == Color.White)
        {
            SendError($"Player: {playerId} is not on turn in game: {GameId}");
        }
                
        if (char.IsUpper(move[0]) && Chess.ActiveColor == Color.Black ||
            char.IsLower(move[0]) && Chess.ActiveColor == Color.White)
        {
            SendError($"Invalid move: {move}");
        }
        
        // Move
        try
        {
            Chess.Move(move);
        } catch (Exception e)
        {
            SendError($"Error while moving in game: {GameId}");
        }

        Console.WriteLine($"Successfully moved in game: {GameId}");
        Game!.UpdateFEN(Chess.GetFen());
        return Game;
    }
    
    // Handlers for events
    private async Task LoadGameState(CancellationToken cancellationToken)
    {
        Console.WriteLine($"GameMasterGrain for game: {GameId} loading game state");
            
        // Load FEN from DB if exists
        using var scope = _serviceScopeFactory.CreateScope();
            
        var tenant = scope.ServiceProvider.GetRequiredService<ITenantFactory>().GetRepository();
        var game = await tenant.Games.TryGetByIdAsync(GameId, cancellationToken);
            
        if (game is null)
        {
            Console.WriteLine($"GameMasterGrain for game: {GameId} game not found");
            return;
        }

        Game = game;
        Chess = Game.Fen.ToChess();

        Console.WriteLine($"GameMasterGrain for game: {GameId} game state loaded");
    }

    private async Task SaveGameState(CancellationToken cancellationToken)
    {
        if (Game is null)
        {
            return;
        }
        
        Console.WriteLine($"GameMasterGrain for game: {GameId} saving game state");
        
        using var scope = _serviceScopeFactory.CreateScope();
        var tenant = scope.ServiceProvider.GetRequiredService<ITenantFactory>().GetRepository();
        
        var currentFen = Chess.GetFen();
        Game.UpdateFEN(currentFen);
        
        await tenant.Games.UpdateAsync(Game, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
        
        Console.WriteLine($"GameMasterGrain for game: {GameId} game state saved");
    }
    
    // Extra methods
    private void SendError(string message, bool stop = false)
    {
        Console.WriteLine(message);
        
        throw new InvalidOperationException(message);
    }
}