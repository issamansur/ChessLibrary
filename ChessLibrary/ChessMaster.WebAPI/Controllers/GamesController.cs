using ChessMaster.Contracts.DTOs.Games;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChessMaster.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public GamesController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        
        _mediator = mediator;
    }
    
    [HttpPost("create", Name = "CreateGame")]
    public async Task<IActionResult> Create(
        [FromBody] CreateGameRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = request.ToCommand();
        Game game = await _mediator.Send(command, cancellationToken);
        var response = game.ToCreateResponse();
        
        return Ok(response);
    }
    
    [HttpGet("{id:guid}", Name = "GetGame")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        var request = new GetGameRequest(id);
        
        var command = request.ToQuery();
        Game game = await _mediator.Send(command, cancellationToken);
        var response = game.ToGetResponse();
        
        return Ok(response);
    }
    
    [HttpGet(Name = "SearchGames")]
    public async Task<IActionResult> Search(
        [FromQuery] SearchGameRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToQuery();
        IEnumerable<Game> games = await _mediator.Send(command, cancellationToken);
        var response = games.ToSearchResponse();
        
        return Ok(response);
    }
    
    [HttpPost("join", Name = "JoinGame")]
    public async Task<IActionResult> Join(
        [FromBody] JoinGameRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = request.ToCommand();
        Game game = await _mediator.Send(command, cancellationToken);
        var response = game.ToCreateResponse();
        
        return Ok(response);
    }
    
    /*
    [HttpPost("move", Name = "MoveGame")]
    public async Task<IActionResult> Move(
        [FromBody] MoveGameRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = request.ToCommand();
        Game game = await _mediator.Send(command, cancellationToken);
        var response = game.ToResponse();
        
        return Ok(response);
    }
    */
}