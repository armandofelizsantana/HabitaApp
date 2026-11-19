using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Favorites.Requests;
using Habita.Application.Dtos.Favorites.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Favorites;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Favorites;

public sealed class GetMyFavoritesUseCase : IGetMyFavoritesUseCase
{
    private readonly IFavoritePropertyRepository _favoriteRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetMyFavoritesRequest> _validator;

    public GetMyFavoritesUseCase(
        IFavoritePropertyRepository favoriteRepository,
        ICurrentUserService currentUser,
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<GetMyFavoritesRequest> validator
    )
    {
        _favoriteRepository = favoriteRepository;
        _currentUser = currentUser;
        _userRepository = userRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<PagedResult<FavoriteResponse>>> ExecuteAsync(
        GetMyFavoritesRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<PagedResult<FavoriteResponse>>();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result<PagedResult<FavoriteResponse>>.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión.")
            );

        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Result<PagedResult<FavoriteResponse>>.Failure(
                Error.Forbidden("Auth.ClientOnly", "Solo los clientes pueden ver sus favoritos.")
            );

        var clientId = _currentUser.UserId;
        var activeAgentIds = await _userRepository.GetActiveIdsByRoleAsync(
            nameof(Roles.Agent),
            cancellationToken
        );

        Expression<Func<FavoriteProperty, bool>> filter = fp =>
            fp.ClientId == clientId && fp.Property != null
            && fp.Property.Status == PropertyStatus.Available && activeAgentIds.Contains(fp.Property.AgentId);
        if (request.Filters is not null)
            filter = request.Filters.Within(filter, fp => fp.Property);

        var options = new QueryOptions<FavoriteProperty>
        {
            Filter = filter,
            Includes =
            [
                fp => fp.Property,
                fp => fp.Property.Images,
                fp => fp.Property.PropertyType!,
                fp => fp.Property.SaleType!,
            ],
            OrderBy = q => q.OrderByDescending(fp => fp.CreatedAt),
            Skip = (request.Page - 1) * request.PageSize,
            Take = request.PageSize,
        };

        var favorites = await _favoriteRepository.GetAllAsync(options, cancellationToken);
        var totalCount = await _favoriteRepository.CountAsync(filter, cancellationToken);

        var items = favorites.Select(fp => _mapper.Map<FavoriteResponse>(fp)).ToList();

        return Result<PagedResult<FavoriteResponse>>.Success(
            new PagedResult<FavoriteResponse>(items, totalCount, request.Page, request.PageSize)
        );
    }
}
