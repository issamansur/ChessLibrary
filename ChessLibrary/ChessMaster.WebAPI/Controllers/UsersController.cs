using ChessMaster.Application.Users.Queries;

namespace ChessMaster.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        
        _mediator = mediator;
    }
    
    [HttpGet("{id:guid}", Name = "GetUser")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetUserQuery(id);
            var command = request.ToCommand();
            User user = await _mediator.Send(command, cancellationToken);
            var response = user.ToResponse();
            return Ok(user);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}