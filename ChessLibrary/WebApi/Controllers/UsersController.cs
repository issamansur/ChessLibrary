namespace WebApi.Controllers;

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
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            User user = await _mediator.Send(new GetUserQuery(id), cancellationToken);
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
    
    [HttpGet("{username}", Name = "GetUserByUsername")]
    public async Task<IActionResult> Get(string username, CancellationToken cancellationToken = default)
    {
        try
        {
            User user = await _mediator.Send(new GetUserByUsernameQuery(username), cancellationToken);
            return Ok(user);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet(Name = "SearchUsers")]
    public async Task<IActionResult> Search(string query, CancellationToken cancellationToken = default)
    {
        try
        {
            IReadOnlyCollection<User> users = await _mediator.Send(new SearchUserQuery(query), cancellationToken);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}