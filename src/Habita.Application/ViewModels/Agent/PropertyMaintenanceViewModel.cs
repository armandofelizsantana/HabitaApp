namespace Habita.Application.ViewModels.Agent;

using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;

public sealed class PropertyMaintenanceViewModel : BaseViewModel
{
    public PagedResult<AgentPropertyListItemViewModel> Properties { get; init; } =
        new([], 0, 1, 12);
}
