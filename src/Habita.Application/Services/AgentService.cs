using Habita.Application.Dtos.Agent.Requests;
using Habita.Application.Dtos.Agent.Responses;
using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Chat.Responses;
using Habita.Application.Dtos.Offers.Requests;
using Habita.Application.Dtos.Offers.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Agent;
using Habita.Application.Interfaces.UseCases.Chat;
using Habita.Application.Interfaces.UseCases.Offers;
using Habita.Domain.Common;
using Habita.Domain.Enums;

namespace Habita.Application.Services;

public sealed class AgentService : IAgentService
{
    private readonly IGetAgentProfileUseCase _getProfileUC;
    private readonly IUpdateAgentProfileUseCase _updateProfileUC;
    private readonly IGetMyConversationsUseCase _chatListUC;
    private readonly IGetConversationUseCase _conversationUC;
    private readonly IReplyMessageUseCase _replyUC;
    private readonly IGetPropertyOffersUseCase _propertyOffersUC;
    private readonly IGetPropertyOfferClientsUseCase _propertyOfferClientsUC;
    private readonly IAcceptOfferUseCase _acceptOfferUC;
    private readonly IRejectOfferUseCase _rejectOfferUC;
    private readonly ICurrentUserService _currentUser;

    public AgentService(
        IGetAgentProfileUseCase getProfileUC,
        IUpdateAgentProfileUseCase updateProfileUC,
        IGetMyConversationsUseCase chatListUC,
        IGetConversationUseCase conversationUC,
        IReplyMessageUseCase replyUC,
        IGetPropertyOffersUseCase propertyOffersUC,
        IGetPropertyOfferClientsUseCase propertyOfferClientsUC,
        IAcceptOfferUseCase acceptOfferUC,
        IRejectOfferUseCase rejectOfferUC,
        ICurrentUserService currentUser
    )
    {
        _getProfileUC = getProfileUC;
        _updateProfileUC = updateProfileUC;
        _chatListUC = chatListUC;
        _conversationUC = conversationUC;
        _replyUC = replyUC;
        _propertyOffersUC = propertyOffersUC;
        _propertyOfferClientsUC = propertyOfferClientsUC;
        _acceptOfferUC = acceptOfferUC;
        _rejectOfferUC = rejectOfferUC;
        _currentUser = currentUser;
    }

    private Result Forbidden(string detail = "Acceso denegado.") =>
        Result.Failure(Error.Forbidden("Auth.Forbidden", detail));

    private Result<T> Forbidden<T>(string detail = "Acceso denegado.") =>
        Result<T>.Failure(Error.Forbidden("Auth.Forbidden", detail));

    public async Task<Result<AgentProfileResponse>> GetProfileAsync(CancellationToken ct = default)
    {
        return await _getProfileUC.ExecuteAsync(new GetAgentProfileRequest(), ct);
    }

    public async Task<Result<AgentProfileResponse>> UpdateProfileAsync(
        UpdateAgentProfileRequest request,
        CancellationToken ct = default
    )
    {
        return await _updateProfileUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<ConversationSummaryResponse>>> GetChatListAsync(
        GetMyConversationsRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Agent)))
            return Forbidden<PagedResult<ConversationSummaryResponse>>(
                "Solo los agentes pueden acceder a sus conversaciones."
            );

        return await _chatListUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<MessageResponse>>> GetConversationAsync(
        GetConversationRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Agent)))
            return Forbidden<PagedResult<MessageResponse>>(
                "Solo los agentes pueden acceder a esta conversación."
            );

        return await _conversationUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<MessageResponse>> ReplyMessageAsync(
        ReplyMessageRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Agent)))
            return Forbidden<MessageResponse>("Solo los agentes pueden responder mensajes.");

        return await _replyUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<OfferResponse>>> GetPropertyOffersAsync(
        GetPropertyOffersRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Agent)))
            return Forbidden<PagedResult<OfferResponse>>(
                "Solo los agentes pueden ver las ofertas de sus propiedades."
            );

        return await _propertyOffersUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<OfferClientSummaryResponse>>> GetPropertyOfferClientsAsync(
        GetPropertyOfferClientsRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Agent)))
            return Forbidden<PagedResult<OfferClientSummaryResponse>>(
                "Solo los agentes pueden ver las ofertas de sus propiedades."
            );

        return await _propertyOfferClientsUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<OfferResponse>> AcceptOfferAsync(
        int offerId,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Agent)))
            return Forbidden<OfferResponse>("Solo los agentes pueden aceptar ofertas.");

        return await _acceptOfferUC.ExecuteAsync(new AcceptOfferRequest(offerId), ct);
    }

    public async Task<Result> RejectOfferAsync(int offerId, CancellationToken ct = default)
    {
        if (!_currentUser.IsInRole(nameof(Roles.Agent)))
            return Forbidden("Solo los agentes pueden rechazar ofertas.");

        return await _rejectOfferUC.ExecuteAsync(new RejectOfferRequest(offerId), ct);
    }
}
