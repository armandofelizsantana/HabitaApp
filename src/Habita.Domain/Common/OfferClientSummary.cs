using Habita.Domain.Enums;

namespace Habita.Domain.Common;

public sealed record OfferClientSummary(
    string ClientId,
    int OfferCount,
    decimal LastAmount,
    OfferStatus LastStatus,
    DateTimeOffset LastCreatedAt
);
