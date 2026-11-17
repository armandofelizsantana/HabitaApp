using System.ComponentModel.DataAnnotations;

namespace Habita.Application.ViewModels.Admin;

using Habita.Application.ViewModels.Shared;

public sealed class ToggleAgentStatusViewModel : BaseViewModel
{
    [Required]
    public string AgentId { get; set; } = null!;

    [Required]
    public bool NewStatus { get; set; }

    public string AgentName { get; init; } = null!;
}
