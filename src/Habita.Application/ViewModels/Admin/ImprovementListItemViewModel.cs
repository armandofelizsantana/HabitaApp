namespace Habita.Application.ViewModels.Admin;

using Habita.Application.ViewModels.Shared;

public sealed class ImprovementListItemViewModel : BaseViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public int PropertiesCount { get; init; }
}
