using Habita.Application.ViewModels.Property;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Habita.Application.Adapters;
using Habita.Application.Dtos.Agent.Requests;
using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Property.Requests;
using Habita.Application.Dtos.Property.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Property;
using Habita.Application.ViewModels.Agent;
using Habita.Application.ViewModels.Shared;
using Habita.Domain.Common;
using Habita.Domain.Enums;
using Habita.Domain.Settings;
using Habita.WebApp.Constants;
using Habita.WebApp.Extensions;
using Habita.WebApp.Filters;

namespace Habita.WebApp.Controllers;

[SessionAuthorize]
[RoleAuthorize(nameof(Roles.Agent))]
public sealed class AgentController : BaseController
{
    private const int DashboardPropertyCount = UIConstants.DashboardPropertyCount;
    private const long ProfileUploadLimit = FileConstants.MaxImageFileSizeBytes + (1024 * 1024);

    private readonly IAgentService _agentService;
    private readonly IGetAgentPropertiesUseCase _getAgentProperties;
    private readonly IViewModelBuilder<BaseViewModel> _viewModelBuilder;
    private readonly IMapper _mapper;

    public AgentController(
        ICurrentUserService currentUser,
        IAgentService agentService,
        IGetAgentPropertiesUseCase getAgentProperties,
        IViewModelBuilder<BaseViewModel> viewModelBuilder,
        IMapper mapper
    )
        : base(currentUser)
    {
        _agentService = agentService;
        _getAgentProperties = getAgentProperties;
        _viewModelBuilder = viewModelBuilder;
        _mapper = mapper;
    }

    [HttpGet]
    [PropertyFilters]
    public async Task<IActionResult> Index([FromQuery] PropertyFilterViewModel filters, CancellationToken cancellationToken = default)
    {
                                                                                       
        var availableResult = await _getAgentProperties.ExecuteAsync(
            new GetAgentPropertiesRequest(1, DashboardPropertyCount, PropertyStatus.Available, filters.ToCriteria()),
            cancellationToken
        );
        var availableTotal = availableResult.IsSuccess ? availableResult.GetValue().TotalCount : 0;
        if (filters.HasCriteria)
        {
            var unfiltered = await _getAgentProperties.ExecuteAsync(
                new GetAgentPropertiesRequest(1, 1, PropertyStatus.Available), cancellationToken);
            if (unfiltered.IsSuccess)
                availableTotal = unfiltered.GetValue().TotalCount;
        }
        var soldResult = await _getAgentProperties.ExecuteAsync(
            new GetAgentPropertiesRequest(1, DashboardPropertyCount, PropertyStatus.Sold),
            cancellationToken
        );
        var chatResult = await _agentService.GetChatListAsync(
            new GetMyConversationsRequest(1, 1),
            cancellationToken
        );

        var viewModel = new AgentHomeViewModel
        {
            AvailableProperties = availableResult.IsSuccess
                ? _mapper.Map<IReadOnlyList<AgentPropertyListItemViewModel>>(
                    availableResult.GetValue().Items
                )
                : [],
            SoldProperties = soldResult.IsSuccess
                ? _mapper.Map<IReadOnlyList<AgentPropertyListItemViewModel>>(soldResult.GetValue().Items)
                : [],
            AvailablePropertiesCount = availableTotal,
            SoldPropertiesCount = soldResult.IsSuccess ? soldResult.GetValue().TotalCount : 0,
            ConversationsCount = chatResult.IsSuccess ? chatResult.GetValue().TotalCount : 0,
        };

        if (availableResult.IsFailure)
            ModelState.AddModelError(string.Empty, availableResult.GetError().Message);
        if (soldResult.IsFailure)
            ModelState.AddModelError(string.Empty, soldResult.GetError().Message);

        viewModel.Filters = filters;
        await this.PopulateBaseViewModelAsync(viewModel, _viewModelBuilder, cancellationToken);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken = default)
    {
        var result = await _agentService.GetProfileAsync(cancellationToken);
        if (result.IsFailure)
        {
            this.SetErrorMessage(result.GetError().Message);
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<AgentProfileViewModel>(result.GetValue());
        await this.PopulateBaseViewModelAsync(viewModel, _viewModelBuilder, cancellationToken);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken = default)
    {
        var result = await _agentService.GetProfileAsync(cancellationToken);
        if (result.IsFailure)
        {
            this.SetErrorMessage(result.GetError().Message);
            return RedirectToAction(nameof(Profile));
        }

        var profile = result.GetValue();
        var viewModel = new UpdateAgentProfileViewModel
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
        UpdateAgentProfileViewModel model,
        IFormFile? photoFile,
        CancellationToken cancellationToken = default
    )
    {
        if (!ModelState.IsValid)
        {
            await this.PopulateBaseViewModelAsync(model, _viewModelBuilder, cancellationToken);
            return View(model);
        }

        var result = await _agentService.UpdateProfileAsync(
            new UpdateAgentProfileRequest(
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
}
