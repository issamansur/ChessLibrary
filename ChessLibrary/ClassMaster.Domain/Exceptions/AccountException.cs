namespace ClassMaster.Domain.Exceptions;

public class AccountException : Exception
{
    public ErrorCodes ErrorCode { get; }

    public AccountException(ErrorCodes errorCode)
        : base(GetErrorMessage(errorCode))
    {
        ErrorCode = errorCode;
    }

    private static string GetErrorMessage(ErrorCodes errorCode)
    {
        switch (errorCode)
        {
            case ErrorCodes.PasswordNotCorrect:
                return "The provided password is not correct.";
            
            default:
                return $"An unknown error occurred. Error code: {errorCode}";
        }
    }
}