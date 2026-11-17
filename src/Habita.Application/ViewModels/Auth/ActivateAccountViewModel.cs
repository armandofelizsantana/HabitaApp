namespace Habita.Application.ViewModels.Auth;

using Habita.Application.ViewModels.Shared;

public sealed class ActivateAccountViewModel : BaseViewModel
{
    public string UserId { get; init; } = null!;
    public string Token { get; init; } = null!;
    public string? Message { get; init; }
    public bool Success { get; init; }
}
