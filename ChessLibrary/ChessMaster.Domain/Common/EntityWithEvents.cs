namespace ChessMaster.Domain.Common;

public abstract class EntityWithEvents
{
    private readonly List<DomainEvent> _domainEvents;

    protected EntityWithEvents() => this._domainEvents = new List<DomainEvent>();

    protected void RaiseEvent(DomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull((object) domainEvent, nameof (domainEvent));
        this._domainEvents.Add(domainEvent);
    }

    public bool HasRaisedEvents => this._domainEvents.Any<DomainEvent>();

    public IEnumerable<DomainEvent> ListRaisedEvents()
    {
        return (IEnumerable<DomainEvent>) this._domainEvents;
    }

    public void ClearEvents() => this._domainEvents.Clear();
}
