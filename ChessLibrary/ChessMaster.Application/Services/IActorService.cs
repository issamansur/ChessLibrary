namespace ChessMaster.Application.Services;

public interface IActorService
{
    void Tell(object message);
    Task<T> Ask<T>(object message);
}