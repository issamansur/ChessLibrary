using ChessMaster.Contracts.DTOs.Users;

namespace ChessMaster.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        
        _mediator = mediator;
    }
    
    [HttpGet("/search",Name = "SearchUsers")]
    public async Task<IActionResult> Search(
        [FromQuery] SearchUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToQuery();
        var users = await _mediator.Send(command, cancellationToken);
        var response = users.ToSearchResponse();
        
        return Ok(response);
    }
    
    [HttpGet("{id:guid}", Name = "GetUser")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        var request = new GetUserRequest(id);
        
        var command = request.ToQuery();
        var user = await _mediator.Send(command, cancellationToken);
        var response = user.ToGetResponse();
        
        return Ok(user);
    }
    
    [HttpGet("{username}", Name = "GetUserByUsername")]
    public async Task<IActionResult> GetByUsername(
        [FromRoute] string username,
        CancellationToken cancellationToken = default)
    {
        var request = new GetUserByUsernameRequest(username);

        var command = request.ToQuery();
        var user = await _mediator.Send(command, cancellationToken);
        var response = user.ToGetResponse();

        return Ok(response);
    }
}