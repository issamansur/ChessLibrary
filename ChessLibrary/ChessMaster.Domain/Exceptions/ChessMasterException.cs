namespace ChessMaster.Domain.Exceptions;

public class ChessMasterException : Exception
{
    public ChessMasterException() { }

    public ChessMasterException(string message)
        : base(message) { }

    public ChessMasterException(string message, Exception inner)
        : base(message, inner) { }
}