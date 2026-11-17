using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;

namespace Habita.Application.ViewModels.Admin;

public sealed class PropertyTypeListViewModel : BaseViewModel
{
    public PagedResult<PropertyTypeListItemViewModel> Items { get; init; } = new([], 0, 1, 20);
    public string? SearchTerm { get; init; }
}
