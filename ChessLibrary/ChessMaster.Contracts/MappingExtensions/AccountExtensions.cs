using ChessMaster.Application.CQRS.Accounts.Commands;
using ChessMaster.Application.CQRS.Accounts.Queries;
using ChessMaster.Contracts.DTOs.Accounts;

namespace ChessMaster.Contracts.MappingExtensions;

public static class AccountExtensions
{
    public static RegisterAccountCommand ToCommand(this RegisterAccountRequest request)
    {
        return new RegisterAccountCommand(
            request.Username,
            request.Email,
            request.Password
        );
    }
    
    public static LoginAccountQuery ToQuery(this LoginAccountRequest request)
    {
        return new LoginAccountQuery(
            request.Login,
            request.Password
        );
    }
    
    public static RegisterAccountResponse ToRegisterResponse(this string token)
    {
        return new RegisterAccountResponse(token);
    }
    
    public static LoginAccountResponse ToLoginResponse(this string token)
    {
        return new LoginAccountResponse(token);
    }
}