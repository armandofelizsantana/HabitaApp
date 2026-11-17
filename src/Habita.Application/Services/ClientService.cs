using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Chat.Responses;
using Habita.Application.Dtos.Client.Requests;
using Habita.Application.Dtos.Client.Responses;
using Habita.Application.Dtos.Favorites.Requests;
using Habita.Application.Dtos.Favorites.Responses;
using Habita.Application.Dtos.Offers.Requests;
using Habita.Application.Dtos.Offers.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Chat;
using Habita.Application.Interfaces.UseCases.Client;
using Habita.Application.Interfaces.UseCases.Favorites;
using Habita.Application.Interfaces.UseCases.Offers;
using Habita.Domain.Common;
using Habita.Domain.Enums;

namespace Habita.Application.Services;

public sealed class ClientService : IClientService
{
    private readonly IGetClientDashboardUseCase _dashboardUC;
    private readonly IGetClientProfileUseCase _getProfileUC;
    private readonly IUpdateClientProfileUseCase _updateProfileUC;
    private readonly IGetMyFavoritesUseCase _favoritesUC;
    private readonly IAddFavoriteUseCase _addFavoriteUC;
    private readonly IRemoveFavoriteUseCase _removeFavoriteUC;
    private readonly ISendMessageUseCase _sendMessageUC;
    private readonly IGetMyConversationsUseCase _chatListUC;
    private readonly IGetConversationUseCase _conversationUC;
    private readonly ICreateOfferUseCase _createOfferUC;
    private readonly IGetMyOffersUseCase _myOffersUC;
    private readonly ICurrentUserService _currentUser;

    public ClientService(
        IGetClientDashboardUseCase dashboardUC,
        IGetClientProfileUseCase getProfileUC,
        IUpdateClientProfileUseCase updateProfileUC,
        IGetMyFavoritesUseCase favoritesUC,
        IAddFavoriteUseCase addFavoriteUC,
        IRemoveFavoriteUseCase removeFavoriteUC,
        ISendMessageUseCase sendMessageUC,
        IGetMyConversationsUseCase chatListUC,
        IGetConversationUseCase conversationUC,
        ICreateOfferUseCase createOfferUC,
        IGetMyOffersUseCase myOffersUC,
        ICurrentUserService currentUser
    )
    {
        _dashboardUC = dashboardUC;
        _getProfileUC = getProfileUC;
        _updateProfileUC = updateProfileUC;
        _favoritesUC = favoritesUC;
        _addFavoriteUC = addFavoriteUC;
        _removeFavoriteUC = removeFavoriteUC;
        _sendMessageUC = sendMessageUC;
        _chatListUC = chatListUC;
        _conversationUC = conversationUC;
        _createOfferUC = createOfferUC;
        _myOffersUC = myOffersUC;
        _currentUser = currentUser;
    }

    private static Result<T> Forbidden<T>(string detail = "Acceso denegado.") =>
        Result<T>.Failure(Error.Forbidden("Auth.Forbidden", detail));

    public async Task<Result<ClientDashboardResponse>> GetDashboardAsync(
        CancellationToken ct = default
    )
    {
        return await _dashboardUC.ExecuteAsync(new GetClientDashboardRequest(), ct);
    }

    public async Task<Result<ClientProfileResponse>> GetProfileAsync(CancellationToken ct = default)
    {
        return await _getProfileUC.ExecuteAsync(new GetClientProfileRequest(), ct);
    }

    public async Task<Result<ClientProfileResponse>> UpdateProfileAsync(
        UpdateClientProfileRequest request,
        CancellationToken ct = default
    )
    {
        return await _updateProfileUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<FavoriteResponse>>> GetFavoritesAsync(
        GetMyFavoritesRequest request,
        CancellationToken ct = default
    )
    {
        return await _favoritesUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<FavoriteResponse>> AddFavoriteAsync(
        int propertyId,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Forbidden<FavoriteResponse>(
                "Solo los clientes pueden agregar propiedades a favoritos."
            );

        return await _addFavoriteUC.ExecuteAsync(new AddFavoriteRequest(propertyId), ct);
    }

    public async Task<Result> RemoveFavoriteAsync(int propertyId, CancellationToken ct = default)
    {
        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Result.Failure(
                Error.Forbidden("Auth.Forbidden", "Solo los clientes pueden eliminar favoritos.")
            );

        return await _removeFavoriteUC.ExecuteAsync(new RemoveFavoriteRequest(propertyId), ct);
    }

    public async Task<Result<MessageResponse>> SendMessageAsync(
        SendMessageRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Forbidden<MessageResponse>("Solo los clientes pueden enviar mensajes.");

        return await _sendMessageUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<ConversationSummaryResponse>>> GetChatListAsync(
        GetMyConversationsRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Forbidden<PagedResult<ConversationSummaryResponse>>(
                "Solo los clientes pueden ver sus conversaciones."
            );

        return await _chatListUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<MessageResponse>>> GetConversationAsync(
        GetConversationRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Forbidden<PagedResult<MessageResponse>>(
                "Solo los clientes pueden ver esta conversación."
            );

        return await _conversationUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<OfferResponse>> CreateOfferAsync(
        CreateOfferRequest request,
        CancellationToken ct = default
    )
    {
        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Forbidden<OfferResponse>("Solo los clientes pueden hacer ofertas.");

        return await _createOfferUC.ExecuteAsync(request, ct);
    }

    public async Task<Result<PagedResult<OfferResponse>>> GetMyOffersAsync(
        GetMyOffersRequest request,
        CancellationToken ct = default
    )
    {
        return await _myOffersUC.ExecuteAsync(request, ct);
    }
}
