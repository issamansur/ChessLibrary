namespace ClassMaster.Domain;

public abstract class DomainEvent
{
    protected DomainEvent(DateTime raiseTime) => this.RaiseTime = raiseTime;

    protected DomainEvent()
    {
    }

    public DateTime RaiseTime { get; private init; }
}
