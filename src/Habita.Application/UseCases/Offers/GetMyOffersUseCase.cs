using AutoMapper;
using FluentValidation;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Offers.Requests;
using Habita.Application.Dtos.Offers.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Offers;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Offers;

public sealed class GetMyOffersUseCase : IGetMyOffersUseCase
{
    private readonly IOfferRepository _offerRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private readonly IValidator<GetMyOffersRequest> _validator;

    public GetMyOffersUseCase(
        IOfferRepository offerRepository,
        ICurrentUserService currentUser,
        IMapper mapper,
        IValidator<GetMyOffersRequest> validator
    )
    {
        _offerRepository = offerRepository;
        _currentUser = currentUser;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<PagedResult<OfferResponse>>> ExecuteAsync(
        GetMyOffersRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<PagedResult<OfferResponse>>();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result<PagedResult<OfferResponse>>.Failure(
                Error.Unauthorized(
                    "Auth.NotAuthenticated",
                    "Debe iniciar sesión para ver sus ofertas."
                )
            );

        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Result<PagedResult<OfferResponse>>.Failure(
                Error.Forbidden("Auth.ClientOnly", "Solo los clientes pueden ver sus ofertas.")
            );

        var options = new QueryOptions<Offer>
        {
            Includes =
            [
                o => o.Property,
                o => o.Property.Images,
                o => o.Property.PropertyType!,
                o => o.Property.SaleType!,
            ],
            OrderBy = q => q.OrderByDescending(o => o.CreatedAt).ThenByDescending(o => o.Id),
            Skip = (request.Page - 1) * request.PageSize,
            Take = request.PageSize,
            Filter = o =>
                (!request.PropertyId.HasValue || o.PropertyId == request.PropertyId.Value)
                && (!request.Status.HasValue || o.Status == request.Status.Value),
        };

        var offers = await _offerRepository.GetByClientAsync(
            _currentUser.UserId,
            options,
            cancellationToken
        );

        var totalCount = await _offerRepository.CountAsync(
            o =>
                o.ClientId == _currentUser.UserId
                && (!request.PropertyId.HasValue || o.PropertyId == request.PropertyId.Value)
                && (!request.Status.HasValue || o.Status == request.Status.Value),
            cancellationToken
        );

        var items = _mapper.Map<List<OfferResponse>>(offers);
        foreach (var item in items)
            item.ClientName = _currentUser.FullName ?? string.Empty;

        return Result<PagedResult<OfferResponse>>.Success(
            new PagedResult<OfferResponse>(items, totalCount, request.Page, request.PageSize)
        );
    }
}
