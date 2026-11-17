namespace Habita.Application.ViewModels.Admin;

using Habita.Application.ViewModels.Shared;

public sealed class DeleteAgentViewModel : BaseViewModel
{
    public string AgentId { get; init; } = null!;
    public string AgentName { get; init; } = null!;
    public string? AgentEmail { get; init; }
    public int PropertiesCount { get; init; }
}
