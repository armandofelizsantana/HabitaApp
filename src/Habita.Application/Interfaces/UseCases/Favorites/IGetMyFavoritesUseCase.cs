using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Favorites.Requests;
using Habita.Application.Dtos.Favorites.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.UseCases.Favorites;

public interface IGetMyFavoritesUseCase
    : IUseCase<GetMyFavoritesRequest, PagedResult<FavoriteResponse>>;
