using ChessMaster.Contracts.DTOs.Accounts;
using Microsoft.AspNetCore.Authorization;

namespace ChessMaster.WebAPI.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AccountsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        
        _mediator = mediator;
    }
    
    [HttpPost("register", Name = "RegisterAccount")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterAccountRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = request.ToCommand(); 
        var token = await _mediator.Send(command, cancellationToken);
        var response = token.ToRegisterResponse();
        
        return Ok(response);
    }
    
    [HttpPost("login", Name = "LoginAccount")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginAccountRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = request.ToQuery();
        var token = await _mediator.Send(command, cancellationToken);
        var response = token.ToLoginResponse();
        
        return Ok(response);
    }
}