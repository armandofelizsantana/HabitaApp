using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Favorites.Requests;
using Habita.Application.Dtos.Favorites.Responses;

namespace Habita.Application.Interfaces.UseCases.Favorites;

public interface IAddFavoriteUseCase : IUseCase<AddFavoriteRequest, FavoriteResponse>;
