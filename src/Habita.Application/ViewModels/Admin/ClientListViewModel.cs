using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;

namespace Habita.Application.ViewModels.Admin;

public sealed class ClientListViewModel : BaseViewModel
{
    public PagedResult<ClientListItemViewModel> Items { get; init; } = new([], 0, 1, 20);
    public string? SearchTerm { get; init; }
}
