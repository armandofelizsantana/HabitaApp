namespace Habita.Application.ViewModels.Chat;

using Habita.Domain.Common;
using Habita.Application.ViewModels.Shared;

public sealed class ChatListViewModel : BaseViewModel
{
    public PagedResult<ConversationSummaryViewModel> Conversations { get; init; } =
        new([], 0, 1, 20);
}
