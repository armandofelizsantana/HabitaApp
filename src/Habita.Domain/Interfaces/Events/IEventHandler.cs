using Habita.Domain.Common;

namespace Habita.Domain.Interfaces.Events;

             
                                                                                     
              
public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}
