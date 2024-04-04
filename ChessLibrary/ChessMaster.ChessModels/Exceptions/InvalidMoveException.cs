namespace ChessMaster.ChessModels.Exceptions;

public class InvalidMoveException : ChessMasterException
{
    public InvalidMoveException() { }

    public InvalidMoveException(string message)
        : base(message) { }

    public InvalidMoveException(string message, Exception inner)
        : base(message, inner) { }
}

public class SelfCaptureException : InvalidMoveException
{
    public SelfCaptureException() { }

    public SelfCaptureException(string message)
        : base(message) { }

    public SelfCaptureException(string message, Exception inner)
        : base(message, inner) { }
}

public class MoveIntoCheckException : InvalidMoveException
{
    public MoveIntoCheckException() { }

    public MoveIntoCheckException(string message)
        : base(message) { }

    public MoveIntoCheckException(string message, Exception inner)
        : base(message, inner) { }
}