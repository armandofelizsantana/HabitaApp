namespace Habita.Application.ViewModels.Agent;

using Habita.Application.ViewModels.Shared;

public sealed class OfferListViewModel : BaseViewModel
{
    public int PropertyId { get; init; }
    public string PropertyCode { get; init; } = null!;
    public IReadOnlyList<Offers.OfferListItemViewModel> Offers { get; init; } = [];
}
