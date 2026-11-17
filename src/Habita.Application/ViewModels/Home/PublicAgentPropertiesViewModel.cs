using Habita.Application.ViewModels.Property;
using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;

namespace Habita.Application.ViewModels.Home;

public sealed class PublicAgentPropertiesViewModel : BaseViewModel
{
    public PublicAgentListItemViewModel Agent { get; init; } = null!;
    public PagedResult<PropertyListItemViewModel> Properties { get; init; } =
        new(Array.Empty<PropertyListItemViewModel>(), 0, 1, 12);
}
