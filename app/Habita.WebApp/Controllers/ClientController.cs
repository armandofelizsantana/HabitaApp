using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Habita.Application.Adapters;
using Habita.Application.Dtos.Client.Requests;
using Habita.Application.Dtos.Favorites.Requests;
using Habita.Application.Dtos.Favorites.Responses;
using Habita.Application.Dtos.Property.Requests;
using Habita.Application.Dtos.Property.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Property;
using Habita.Application.ViewModels.Client;
using Habita.Application.ViewModels.Property;
using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;
using Habita.Domain.Enums;
using Habita.Domain.Settings;
using Habita.WebApp.Constants;
using Habita.WebApp.Extensions;
using Habita.WebApp.Filters;

namespace Habita.WebApp.Controllers;

[SessionAuthorize]
[RoleAuthorize(nameof(Roles.Client))]
public sealed class ClientController : BaseController
{
    private const int DashboardPropertyCount = UIConstants.DashboardPropertyCount;
    private const int PageSize = UIConstants.DefaultPageSize;
    private const long ProfileUploadLimit = FileConstants.MaxImageFileSizeBytes + (1024 * 1024);

    private readonly IClientService _clientService;
    private readonly IGetPropertyListUseCase _getPropertyList;
    private readonly IViewModelBuilder<BaseViewModel> _viewModelBuilder;
    private readonly IMapper _mapper;

    public ClientController(
        ICurrentUserService currentUser,
        IClientService clientService,
        IGetPropertyListUseCase getPropertyList,
        IViewModelBuilder<BaseViewModel> viewModelBuilder,
        IMapper mapper
    )
        : base(currentUser)
    {
        _clientService = clientService;
        _getPropertyList = getPropertyList;
        _viewModelBuilder = viewModelBuilder;
        _mapper = mapper;
    }

    [HttpGet]
    [PropertyFilters]
    public async Task<IActionResult> Index([FromQuery] PropertyFilterViewModel filters, CancellationToken cancellationToken = default)
    {
        var dashboardResult = await _clientService.GetDashboardAsync(cancellationToken);
        var propertiesResult = await _getPropertyList.ExecuteAsync(
            filters.ToListRequest(1, DashboardPropertyCount),
            cancellationToken
        );

        var viewModel = dashboardResult.IsSuccess
            ? _mapper.Map<ClientDashboardViewModel>(dashboardResult.GetValue())
            : new ClientDashboardViewModel();

        if (dashboardResult.IsFailure)
            ModelState.AddModelError(string.Empty, dashboardResult.GetError().Message);

        if (propertiesResult.IsSuccess)
        {
            viewModel = new ClientDashboardViewModel
            {
                FavoritesCount = viewModel.FavoritesCount,
                TotalOffers = viewModel.TotalOffers,
                ActiveOffers = viewModel.ActiveOffers,
                ConversationsCount = viewModel.ConversationsCount,
                TotalMessages = viewModel.TotalMessages,
                AvailableProperties = _mapper.Map<IReadOnlyList<PropertyListItemViewModel>>(
                    propertiesResult.GetValue().Items
                ),
            };
        }
        else
        {
            ModelState.AddModelError(
                string.Empty,
                "No se pudieron cargar propiedades destacadas. Intenta nuevamente."
            );
        }

        viewModel.Filters = filters;
        await this.PopulateBaseViewModelAsync(viewModel, _viewModelBuilder, cancellationToken);
        return View(viewModel);
    }

    [HttpGet]
    [PropertyFilters]
    public async Task<IActionResult> Favorites([FromQuery] PropertyFilterViewModel filters, int page = 1, CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        var result = await _clientService.GetFavoritesAsync(
            new GetMyFavoritesRequest(page, PageSize, filters.ToCriteria()),
            cancellationToken
        );

        if (result.IsSuccess && result.GetValue().TotalPages > 0 && page > result.GetValue().TotalPages)
        {
            var route = filters.RouteValues();
            route["page"] = result.GetValue().TotalPages;
            return RedirectToAction(nameof(Favorites), route);
        }

        var favorites = result.IsSuccess
            ? MapPage(result.GetValue())
            : new PagedResult<FavoriteListItemViewModel>([], 0, page, PageSize);
        if (result.IsFailure)
            ModelState.AddModelError(string.Empty, result.GetError().Message);

        var viewModel = new MyFavoritesViewModel { Filters = filters, Favorites = favorites };
        await this.PopulateBaseViewModelAsync(viewModel, _viewModelBuilder, cancellationToken);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFavorite(
        int propertyId,
        int page = 1,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _clientService.RemoveFavoriteAsync(propertyId, cancellationToken);
        if (result.IsSuccess)
            this.SetSuccessMessage("Propiedad eliminada de tus favoritos.");
        else
            this.SetErrorMessage(result.GetError().Message);

        return RedirectToAction(nameof(Favorites), new { page = Math.Max(1, page) });
    }

    [HttpGet]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken = default)
    {
        var result = await _clientService.GetProfileAsync(cancellationToken);
        if (result.IsFailure)
        {
            this.SetErrorMessage(result.GetError().Message);
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<ClientProfileViewModel>(result.GetValue());
        await this.PopulateBaseViewModelAsync(viewModel, _viewModelBuilder, cancellationToken);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken = default)
    {
        var result = await _clientService.GetProfileAsync(cancellationToken);
        if (result.IsFailure)
        {
            this.SetErrorMessage(result.GetError().Message);
            return RedirectToAction(nameof(Profile));
        }

        var profile = result.GetValue();
        var viewModel = new UpdateClientProfileViewModel
        {
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Phone = profile.Phone ?? string.Empty,
        };
        await this.PopulateBaseViewModelAsync(viewModel, _viewModelBuilder, cancellationToken);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(ProfileUploadLimit)]
    public async Task<IActionResult> Edit(
        UpdateClientProfileViewModel model,
        IFormFile? photoFile,
        CancellationToken cancellationToken = default
    )
    {
        if (!ModelState.IsValid)
        {
            await this.PopulateBaseViewModelAsync(model, _viewModelBuilder, cancellationToken);
            return View(model);
        }

        var result = await _clientService.UpdateProfileAsync(
            new UpdateClientProfileRequest(
                model.FirstName,
                model.LastName,
                model.Phone,
                photoFile.ToAppFile()
            ),
            cancellationToken
        );
        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.GetError().Message);
            await this.PopulateBaseViewModelAsync(model, _viewModelBuilder, cancellationToken);
            return View(model);
        }

        this.SetSuccessMessage("Perfil actualizado correctamente.");
        return RedirectToAction(nameof(Profile));
    }

    private PagedResult<FavoriteListItemViewModel> MapPage(PagedResult<FavoriteResponse> page) =>
        new(
            _mapper.Map<IReadOnlyList<FavoriteListItemViewModel>>(page.Items),
            page.TotalCount,
            page.Page,
            page.PageSize
        );
}
