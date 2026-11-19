namespace Habita.Application.ViewModels.Agent;

using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;

public sealed class PropertyMaintenanceViewModel : BaseViewModel
{
    public Habita.Application.ViewModels.Property.PropertyFilterViewModel Filters { get; set; } = new();

    public PagedResult<AgentPropertyListItemViewModel> Properties { get; init; } =
        new([], 0, 1, 12);
    public IReadOnlyList<AgentPropertyListItemViewModel> SoldProperties { get; init; } = [];
    public int SoldPropertiesCount { get; init; }
}
