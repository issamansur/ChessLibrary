using System.Security.Claims;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Contracts.DTOs.Games;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;

namespace ChessMaster.WebAPI.Controllers;

[ApiController]
[Route("api/games")]
[Authorize]
public class GamesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IChessActorService _chessActorService;
    
    public GamesController(IMediator mediator, IChessActorService chessActorService)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        
        _mediator = mediator;
        _chessActorService = chessActorService;
    }
    
    [HttpPost("create", Name = "CreateGame")]
    public async Task<IActionResult> Create(
        [FromBody] CreateGameRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var userId = new Guid(User.FindFirstValue(CustomClaimTypes.UserId));
        
        var command = request.ToCommand(userId);
        var game = await _mediator.Send(command, cancellationToken);
        var response = game.ToCreateResponse();
        
        return Ok(response);
    }
    
    [HttpPost("join", Name = "JoinGame")]
    public async Task<IActionResult> Join(
        [FromBody] JoinGameRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var userId = new Guid(User.FindFirstValue(CustomClaimTypes.UserId));
        
        var command = request.ToCommand(userId);
        await _chessActorService.JoinGameAsync(command, cancellationToken);
        
        return Ok();
    }
    
    [HttpPost("move", Name = "MoveGame")]
    [Authorize]
    public async Task<IActionResult> Move(
        [FromBody] MoveGameRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var userId = new Guid(User.FindFirstValue(CustomClaimTypes.UserId));
        
        var command = request.ToCommand(userId);
        Game game = await _chessActorService.MoveGameAsync(command, cancellationToken);
        var response = game.ToMoveResponse();
        
        return Ok(response);
    }
    
    [HttpGet("{id:guid}", Name = "GetGame")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default
    )
    {
        var request = new GetGameRequest(id);
        
        var command = request.ToQuery();
        var game = await _mediator.Send(command, cancellationToken);
        var response = game.ToGetResponse();
        
        return Ok(response);
    }
    
    [HttpGet(Name = "SearchGames")]
    [AllowAnonymous]
    public async Task<IActionResult> Search(
        [FromQuery] SearchGameRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = request.ToQuery();
        var games = await _mediator.Send(command, cancellationToken);
        var response = games.ToSearchResponse();
        
        return Ok(response);
    }
}