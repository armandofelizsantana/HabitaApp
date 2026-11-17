using Habita.Domain.Common;

namespace Habita.Domain.Interfaces.Events;

             
                                                                             
              
public interface IDomainEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : IDomainEvent;
}
