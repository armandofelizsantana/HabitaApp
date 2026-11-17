namespace Habita.Domain.Common;

             
                                                                     
              
public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}
