namespace Habita.Domain.Common;

             
                                                                         
                                       
              
public abstract class AggregateRoot : BaseEntity<int>
{
    private readonly List<IDomainEvent> _events = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    protected void RaiseEvent(IDomainEvent @event) => _events.Add(@event);

    public void ClearDomainEvents() => _events.Clear();
}
