namespace Habita.Application.ViewModels.Offers;

using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;
using Habita.Domain.Enums;

public sealed class MyOffersViewModel : BaseViewModel
{
    public PagedResult<OfferListItemViewModel> Offers { get; init; } = new([], 0, 1, 12);
    public int? PropertyId { get; init; }
    public OfferStatus? Status { get; init; }
}
