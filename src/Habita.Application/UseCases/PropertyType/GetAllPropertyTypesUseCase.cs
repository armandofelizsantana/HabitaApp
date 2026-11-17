using AutoMapper;
using FluentValidation;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Catalog;

public sealed class GetAllPropertyTypesUseCase : IGetAllPropertyTypesUseCase
{
    private readonly IGenericRepository<PropertyType> _repository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetAllPropertyTypesRequest> _validator;

    public GetAllPropertyTypesUseCase(
        IGenericRepository<PropertyType> repository,
        IPropertyRepository propertyRepository,
        IMapper mapper,
        ICurrentUserService currentUser,
        IValidator<GetAllPropertyTypesRequest> validator
    )
    {
        _repository = repository;
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _currentUser = currentUser;
        _validator = validator;
    }

    public async Task<Result<PagedResult<PropertyTypeResponse>>> ExecuteAsync(
        GetAllPropertyTypesRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<PagedResult<PropertyTypeResponse>>();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result<PagedResult<PropertyTypeResponse>>.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión.")
            );

        if (
            !_currentUser.IsInRole(nameof(Roles.Admin))
            && !_currentUser.IsInRole(nameof(Roles.Agent))
        )
            return Result<PagedResult<PropertyTypeResponse>>.Failure(
                Error.Forbidden(
                    "Auth.AdminOrAgent",
                    "Solo administradores o agentes pueden listar tipos de propiedad."
                )
            );

        string? term = null;
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            term = request.SearchTerm.Trim().ToLowerInvariant();
        }

        var options = new QueryOptions<PropertyType>
        {
            OrderBy = q => q.OrderBy(pt => pt.Name),
            Skip = (request.Page - 1) * request.PageSize,
            Take = request.PageSize,
        };

        if (term is not null)
        {
            options.Filter = pt =>
                pt.Name.ToLower().Contains(term) || pt.Description.ToLower().Contains(term);
        }

        var items = await _repository.GetAllAsync(options, cancellationToken);

        var totalCount = term is not null
            ? await _repository.CountAsync(
                pt => pt.Name.ToLower().Contains(term) || pt.Description.ToLower().Contains(term),
                cancellationToken
            )
            : await _repository.CountAsync(null, cancellationToken);

        var responseItems = _mapper.Map<IReadOnlyList<PropertyTypeResponse>>(items);

        var enrichedItems = new List<PropertyTypeResponse>(responseItems.Count);
        foreach (var item in responseItems)
        {
            var count = await _propertyRepository.CountAsync(
                p => p.PropertyTypeId == item.Id,
                cancellationToken
            );
            enrichedItems.Add(item with { PropertiesCount = count });
        }

        var pagedResult = new PagedResult<PropertyTypeResponse>(
            enrichedItems.AsReadOnly(),
            totalCount,
            request.Page,
            request.PageSize
        );

        return Result<PagedResult<PropertyTypeResponse>>.Success(pagedResult);
    }
}
