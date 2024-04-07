namespace ChessMaster.Domain.Events;

public class PasswordResetRequestEvent : DomainEvent
{
    public Guid AccountId { get; }

    public PasswordResetRequestEvent(Guid accountId, DateTime raiseTime)
        : base(raiseTime)
    {
        AccountId = accountId;
    }
}