using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;

namespace Habita.Application.ViewModels.Admin;

public sealed class AgentListViewModel : BaseViewModel
{
    public PagedResult<AgentListItemViewModel> Items { get; init; } = new([], 0, 1, 20);
    public string? SearchTerm { get; init; }
}
