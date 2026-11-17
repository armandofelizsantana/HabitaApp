using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Chat.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.UseCases.Chat;

public interface IGetConversationUseCase
    : IUseCase<GetConversationRequest, PagedResult<MessageResponse>>;
