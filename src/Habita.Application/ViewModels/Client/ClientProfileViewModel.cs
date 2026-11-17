namespace Habita.Application.ViewModels.Client;

using Habita.Application.ViewModels.Shared;

public sealed class ClientProfileViewModel : BaseViewModel
{
    public string Id { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? Phone { get; init; }
    public string? ProfileImage { get; init; }
}
