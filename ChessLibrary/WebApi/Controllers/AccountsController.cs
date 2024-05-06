using ChessMaster.Application.Accounts.Commands;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController
{
    private readonly IMediator _mediator;
    
    public AccountsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        
        _mediator = mediator;
    }
}