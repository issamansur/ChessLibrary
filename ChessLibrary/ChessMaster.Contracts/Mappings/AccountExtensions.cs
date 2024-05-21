using ChessMaster.Application.Accounts.Commands;
using ChessMaster.Application.Accounts.Queries;

using ChessMaster.Contracts.DTOs.Accounts;

namespace ChessMaster.Contracts.Mappings;

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
    
    public static RegisterAccountResponse ToRegisterResponse (this Account account)
    {
        return new RegisterAccountResponse(account);
    }
    
    public static LoginAccountResponse ToLoginResponse(this Account account)
    {
        return new LoginAccountResponse(account);
    }
}