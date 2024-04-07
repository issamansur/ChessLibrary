namespace ChessMaster.Domain.Events;

public class CreatedEvent : DomainEvent
{
    public Guid AccountId { get; }

    public CreatedEvent(Guid accountId, DateTime raiseTime)
        : base(raiseTime)
    {
        AccountId = accountId;
    }
}