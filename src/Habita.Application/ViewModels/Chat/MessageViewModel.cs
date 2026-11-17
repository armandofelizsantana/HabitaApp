using Habita.Domain.Enums;

namespace Habita.Application.ViewModels.Chat;

using Habita.Application.ViewModels.Shared;

public sealed class MessageViewModel : BaseViewModel
{
    public int Id { get; init; }
    public int PropertyId { get; init; }
    public string PropertyCode { get; init; } = null!;
    public string SenderId { get; init; } = null!;
    public string SenderName { get; init; } = null!;
    public SenderType SenderType { get; init; }
    public string Content { get; init; } = null!;
    public DateTimeOffset SentAt { get; init; }
    public bool IsFromCurrentUser { get; set; }
}
