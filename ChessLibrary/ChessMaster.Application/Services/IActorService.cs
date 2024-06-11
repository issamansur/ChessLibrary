namespace ChessMaster.Application.Services;

public interface IActorService
{
    Task Tell<T>(T message, CancellationToken cancellationToken);
    Task<TResult> Ask<TRequest, TResult>(TRequest message, CancellationToken cancellationToken);
}