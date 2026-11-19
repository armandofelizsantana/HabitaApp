namespace Habita.Application.ViewModels.Admin;

using Habita.Application.ViewModels.Shared;

public sealed class ClientListItemViewModel : BaseViewModel
{
    public string Id { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; }
    public string FullName => $"{FirstName} {LastName}";
    public string StatusLabel => IsActive ? "Activo" : "Inactivo";
}
