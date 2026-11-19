using AutoMapper;
using FluentValidation;
using Habita.Application.Common.Interfaces;
using Habita.Application.Common.Validation;
using Habita.Application.Interfaces.Services;
using Habita.Domain.Common;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.Services;

public abstract class CatalogLookupService<TEntity, TRequest, TResponse>(
    IGenericRepository<TEntity> repository,
    IMapper mapper,
    ICurrentUserService currentUser,
    IValidator<TRequest> validator
) : IUseCase<TRequest, TResponse>
    where TEntity : BaseEntity<int>
    where TRequest : notnull
{
    protected abstract int GetId(TRequest request);
    protected abstract Error ForbiddenError { get; }
    protected abstract Error MissingError { get; }

    public async Task<Result<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToResult<TResponse>();

        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<TResponse>.Failure(Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión."));

        if (!currentUser.IsInRole(nameof(Roles.Admin)) && !currentUser.IsInRole(nameof(Roles.Developer)))
            return Result<TResponse>.Failure(ForbiddenError);

        var entity = await repository.GetByIdAsync(GetId(request), cancellationToken);
        return entity is null
            ? Result<TResponse>.Failure(MissingError)
            : Result<TResponse>.Success(mapper.Map<TResponse>(entity));
    }
}
