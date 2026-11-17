using Microsoft.AspNetCore.Mvc;
using Habita.Application.Interfaces.Services;
using Habita.Application.ViewModels.Admin;
using Habita.Application.ViewModels.Shared;
using Habita.Domain.Enums;
using Habita.WebApp.Extensions;
using Habita.WebApp.Filters;

namespace Habita.WebApp.Controllers;

[SessionAuthorize]
[RoleAuthorize(nameof(Roles.Admin))]
public sealed class DashboardController : BaseController
{
    private readonly IAdminService _adminService;
    private readonly IViewModelBuilder<BaseViewModel> _viewModelBuilder;

    public DashboardController(
        ICurrentUserService currentUser,
        IAdminService adminService,
        IViewModelBuilder<BaseViewModel> viewModelBuilder
    )
        : base(currentUser)
    {
        _adminService = adminService;
        _viewModelBuilder = viewModelBuilder;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        var result = await _adminService.GetDashboardAsync(cancellationToken);

        var viewModel = new AdminDashboardViewModel
        {
            AvailableProperties = result.IsSuccess ? result.GetValue().AvailableProperties : 0,
            SoldProperties = result.IsSuccess ? result.GetValue().SoldProperties : 0,
            ActiveAgents = result.IsSuccess ? result.GetValue().ActiveAgents : 0,
            InactiveAgents = result.IsSuccess ? result.GetValue().InactiveAgents : 0,
            ActiveClients = result.IsSuccess ? result.GetValue().ActiveClients : 0,
            InactiveClients = result.IsSuccess ? result.GetValue().InactiveClients : 0,
            ActiveDevelopers = result.IsSuccess ? result.GetValue().ActiveDevelopers : 0,
            InactiveDevelopers = result.IsSuccess ? result.GetValue().InactiveDevelopers : 0,
        };

        if (result.IsFailure)
            this.SetErrorMessage(result.GetError().Message);

        await this.PopulateBaseViewModelAsync(viewModel, _viewModelBuilder, cancellationToken);
        return View(viewModel);
    }
}
