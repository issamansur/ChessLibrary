using ChessMaster.Contracts.DTOs.Accounts;

namespace ChessMaster.WebAPI.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AccountsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        
        _mediator = mediator;
    }
    
    [HttpPost("register", Name = "RegisterAccount")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterAccountRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = request.ToCommand(); 
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost("login", Name = "LoginAccount")]
    public async Task<IActionResult> Login(
        [FromBody] LoginAccountRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = request.ToQuery();
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}