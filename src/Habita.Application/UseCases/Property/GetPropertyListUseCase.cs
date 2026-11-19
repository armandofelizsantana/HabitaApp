using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Property.Requests;
using Habita.Application.Dtos.Property.Responses;
using Habita.Application.Interfaces.UseCases.Property;
using Habita.Application.Interfaces.Services;
using Habita.Domain.Common;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;
using Habita.Domain.ValueObjects;
using PropertyEntity = Habita.Domain.Entities.Property;

namespace Habita.Application.UseCases.Property;

public sealed class GetPropertyListUseCase : IGetPropertyListUseCase
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFavoritePropertyRepository _favoriteRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private readonly IValidator<GetPropertyListRequest> _validator;

    public GetPropertyListUseCase(
        IPropertyRepository propertyRepository,
        IUserRepository userRepository,
        IFavoritePropertyRepository favoriteRepository,
        ICurrentUserService currentUser,
        IMapper mapper,
        IValidator<GetPropertyListRequest> validator
    )
    {
        _propertyRepository = propertyRepository;
        _userRepository = userRepository;
        _favoriteRepository = favoriteRepository;
        _currentUser = currentUser;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<PagedResult<PropertyListItemResponse>>> ExecuteAsync(
        GetPropertyListRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<PagedResult<PropertyListItemResponse>>();

        var activeAgentIds = await _userRepository.GetActiveIdsByRoleAsync(
            nameof(Roles.Agent),
            cancellationToken
        );
        var filter = BuildFilter(request, activeAgentIds);

        var options = new QueryOptions<PropertyEntity>
        {
            Filter = filter,
            Includes = [p => p.PropertyType!, p => p.SaleType!, p => p.Images],
            OrderBy = q => q.OrderByDescending(p => p.CreatedAt),
            Skip = (request.Page - 1) * request.PageSize,
            Take = request.PageSize,
        };

        var properties = await _propertyRepository.GetAllAsync(options, cancellationToken);
        var totalCount = await _propertyRepository.CountAsync(filter, cancellationToken);

        var items = _mapper.Map<List<PropertyListItemResponse>>(properties);
        var agentIds = properties.Select(p => p.AgentId).Distinct().ToList();
        var agents = await _userRepository.GetByIdsAsync(agentIds, cancellationToken);
        var agentMap = agents.ToDictionary(a => a.Id);

        foreach (var item in items)
        {
            var prop = properties.First(p => p.Id == item.Id);
            if (agentMap.TryGetValue(prop.AgentId, out var agent))
                item.AgentName = $"{agent.FirstName} {agent.LastName}".Trim();
        }

        if (
            _currentUser.IsAuthenticated
            && _currentUser.UserId is not null
            && _currentUser.IsInRole(nameof(Roles.Client))
        )
        {
            var favoriteIds = await _favoriteRepository.GetPropertyIdsAsync(
                _currentUser.UserId,
                items.Select(item => item.Id).ToArray(),
                cancellationToken
            );
            foreach (var item in items)
                item.IsFavorite = favoriteIds.Contains(item.Id);
        }

        return Result<PagedResult<PropertyListItemResponse>>.Success(
            new PagedResult<PropertyListItemResponse>(
                items,
                totalCount,
                request.Page,
                request.PageSize
            )
        );
    }

    private static Expression<Func<PropertyEntity, bool>> BuildFilter(
        GetPropertyListRequest req,
        IReadOnlySet<string> activeAgentIds
    )
    {
        var isCodeSearch = !string.IsNullOrWhiteSpace(req.SearchTerm)
            && req.SearchTerm.Length == 6
            && req.SearchTerm.All(char.IsDigit);

        Expression<Func<PropertyEntity, bool>> scope = p =>
            (req.IncludeAllStatuses || activeAgentIds.Contains(p.AgentId))
            && (req.IncludeAllStatuses || p.Status == PropertyStatus.Available)
            && (string.IsNullOrWhiteSpace(req.SearchTerm)
                || p.Description.Contains(req.SearchTerm)
                || (isCodeSearch && p.Code == PropertyCode.Unsafe(req.SearchTerm)))
            && (!req.SizeMin.HasValue || p.Size.Area >= req.SizeMin.Value)
            && (!req.SizeMax.HasValue || p.Size.Area <= req.SizeMax.Value)
            && (!req.SaleTypeId.HasValue || p.SaleTypeId == req.SaleTypeId.Value)
            && (string.IsNullOrWhiteSpace(req.AgentId) || p.AgentId == req.AgentId);

        var criteria = new PropertySearchCriteria(req.PropertyTypeId, req.PriceMin, req.PriceMax,
            req.Bedrooms, req.Bathrooms, req.Code);
        return criteria.Within(scope, p => p);
    }
}
