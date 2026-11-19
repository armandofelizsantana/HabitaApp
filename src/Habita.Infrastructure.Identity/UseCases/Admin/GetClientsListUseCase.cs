using FluentValidation;
using Microsoft.Extensions.Logging;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Admin.Requests;
using Habita.Application.Dtos.Admin.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Admin;
using Habita.Domain.Common;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Infrastructure.Identity.UseCases.Admin;

public sealed class GetClientsListUseCase : IGetClientsListUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetClientsListRequest> _validator;
    private readonly ILogger<GetClientsListUseCase> _logger;

    public GetClientsListUseCase(
        IUserRepository userRepository,
        ICurrentUserService currentUser,
        IValidator<GetClientsListRequest> validator,
        ILogger<GetClientsListUseCase> logger
    )
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedResult<ClientListItemResponse>>> ExecuteAsync(
        GetClientsListRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToResult<PagedResult<ClientListItemResponse>>();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result<PagedResult<ClientListItemResponse>>.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión.")
            );

        if (!_currentUser.IsInRole(nameof(Roles.Admin)))
            return Result<PagedResult<ClientListItemResponse>>.Failure(
                Error.Forbidden("Auth.AdminOnly", "Solo administradores pueden gestionar clientes.")
            );

        var page = await _userRepository.GetByRoleAsync(
            nameof(Roles.Client),
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken
        );

        var items = page.Items.Select(user => new ClientListItemResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.UserName,
                user.Phone,
                user.IsActive
            ))
            .ToList();

        _logger.LogInformation(
            "Admin {AdminId} consultó el directorio de clientes (página {Page}).",
            _currentUser.UserId,
            request.Page
        );

        return Result<PagedResult<ClientListItemResponse>>.Success(
            new PagedResult<ClientListItemResponse>(items, page.TotalCount, page.Page, page.PageSize)
        );
    }
}
