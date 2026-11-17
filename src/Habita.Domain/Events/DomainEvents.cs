using Habita.Domain.Common;

namespace Habita.Domain.Events;

public sealed record OfferAcceptedEvent(
    int OfferId,
    int PropertyId,
    string ClientId,
    decimal Amount,
    DateTimeOffset OccurredOn
) : IDomainEvent;
