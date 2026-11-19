namespace Habita.Application.ViewModels.Agent;

using Habita.Application.ViewModels.Shared;

public sealed class AgentHomeViewModel : BaseViewModel
{
    public Habita.Application.ViewModels.Property.PropertyFilterViewModel Filters { get; set; } = new();

    public IReadOnlyList<AgentPropertyListItemViewModel> AvailableProperties { get; init; } = [];
    public IReadOnlyList<AgentPropertyListItemViewModel> SoldProperties { get; init; } = [];
    public int AvailablePropertiesCount { get; init; }
    public int SoldPropertiesCount { get; init; }
    public int ConversationsCount { get; init; }
    public int TotalProperties => AvailablePropertiesCount + SoldPropertiesCount;
}
