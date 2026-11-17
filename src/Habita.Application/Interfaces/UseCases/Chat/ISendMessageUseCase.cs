using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Chat.Responses;

namespace Habita.Application.Interfaces.UseCases.Chat;

public interface ISendMessageUseCase : IUseCase<SendMessageRequest, MessageResponse>;
