using ChessMaster.Contracts.DTOs.Accounts.Requests;
using ChessMaster.Contracts.DTOs.Accounts.Responses;

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
    
    public static LoginAccountCommand ToCommand(this LoginAccountRequest request)
    {
        return new LoginAccountCommand(
            request.Login,
            request.Password
        );
    }
    
    public static RegisterAccountResponse ToRegisterResponse (this string token)
    {
        return new RegisterAccountResponse(token);
    }
    
    public static LoginAccountResponse ToLoginResponse(this string token)
    {
        return new LoginAccountResponse(token);
    }
}