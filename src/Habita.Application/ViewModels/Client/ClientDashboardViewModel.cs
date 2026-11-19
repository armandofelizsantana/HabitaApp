namespace Habita.Application.ViewModels.Client;

using Habita.Application.ViewModels.Shared;

public sealed class ClientDashboardViewModel : BaseViewModel
{
    public Habita.Application.ViewModels.Property.PropertyFilterViewModel Filters { get; set; } = new();

    public int FavoritesCount { get; init; }
    public int TotalOffers { get; init; }
    public int ActiveOffers { get; init; }
    public int ConversationsCount { get; init; }
    public int TotalMessages { get; init; }
    public IReadOnlyList<Property.PropertyListItemViewModel> AvailableProperties { get; init; } =
    [];
}
