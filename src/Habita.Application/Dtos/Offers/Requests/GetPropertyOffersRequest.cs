using Habita.Domain.Enums;

namespace Habita.Application.Dtos.Offers.Requests;

public sealed record GetPropertyOffersRequest(
    int PropertyId,
    int Page = 1,
    int PageSize = 20,
    OfferStatus? Status = null,
    string? ClientId = null
);
