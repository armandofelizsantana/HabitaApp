using FluentValidation;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Catalog;

public sealed class GetPublicPropertyTypesUseCase : IGetPublicPropertyTypesUseCase
{
    private readonly IGenericRepository<PropertyType> _repository;
    private readonly IValidator<GetPublicPropertyTypesRequest> _validator;

    public GetPublicPropertyTypesUseCase(
        IGenericRepository<PropertyType> repository,
        IValidator<GetPublicPropertyTypesRequest> validator
    )
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<IReadOnlyList<PropertyTypeResponse>>> ExecuteAsync(
        GetPublicPropertyTypesRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<IReadOnlyList<PropertyTypeResponse>>();

        var types = await _repository.GetAllAsync(
            new QueryOptions<PropertyType> { OrderBy = query => query.OrderBy(type => type.Name) },
            cancellationToken
        );

        return Result<IReadOnlyList<PropertyTypeResponse>>.Success(
            types.Select(type => new PropertyTypeResponse(type.Id, type.Name, type.Description)).ToList()
        );
    }
}
