using System.ComponentModel.DataAnnotations;

namespace Habita.Application.ViewModels.Admin;

using Habita.Application.ViewModels.Shared;

public sealed class ToggleAdminStatusViewModel : BaseViewModel
{
    [Required]
    public string AdminId { get; set; } = null!;

    [Required]
    public bool NewStatus { get; set; }

    public string AdminName { get; init; } = null!;
}
