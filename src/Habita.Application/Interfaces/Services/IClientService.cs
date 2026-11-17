using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Chat.Responses;
using Habita.Application.Dtos.Client.Requests;
using Habita.Application.Dtos.Client.Responses;
using Habita.Application.Dtos.Favorites.Requests;
using Habita.Application.Dtos.Favorites.Responses;
using Habita.Application.Dtos.Offers.Requests;
using Habita.Application.Dtos.Offers.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.Services;

public interface IClientService
{
    Task<Result<ClientDashboardResponse>> GetDashboardAsync(CancellationToken ct = default);
    Task<Result<ClientProfileResponse>> GetProfileAsync(CancellationToken ct = default);
    Task<Result<ClientProfileResponse>> UpdateProfileAsync(
        UpdateClientProfileRequest request,
        CancellationToken ct = default
    );
    Task<Result<PagedResult<FavoriteResponse>>> GetFavoritesAsync(
        GetMyFavoritesRequest request,
        CancellationToken ct = default
    );
    Task<Result<FavoriteResponse>> AddFavoriteAsync(int propertyId, CancellationToken ct = default);
    Task<Result> RemoveFavoriteAsync(int propertyId, CancellationToken ct = default);
    Task<Result<PagedResult<ConversationSummaryResponse>>> GetChatListAsync(
        GetMyConversationsRequest request,
        CancellationToken ct = default
    );
    Task<Result<MessageResponse>> SendMessageAsync(
        SendMessageRequest request,
        CancellationToken ct = default
    );
    Task<Result<PagedResult<MessageResponse>>> GetConversationAsync(
        GetConversationRequest request,
        CancellationToken ct = default
    );
    Task<Result<OfferResponse>> CreateOfferAsync(
        CreateOfferRequest request,
        CancellationToken ct = default
    );
    Task<Result<PagedResult<OfferResponse>>> GetMyOffersAsync(
        GetMyOffersRequest request,
        CancellationToken ct = default
    );
}
