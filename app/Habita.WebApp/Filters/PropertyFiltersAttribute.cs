using AutoMapper;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Application.ViewModels.Property;
using Habita.Application.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Habita.WebApp.Filters;

public sealed class PropertyFiltersAttribute() : TypeFilterAttribute(typeof(LoadPropertyFilters));

public sealed class LoadPropertyFilters(IGetPublicPropertyTypesUseCase types, IMapper mapper) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var filters = (PropertyFilterViewModel)context.ActionArguments["filters"]!;
        filters.Code = filters.Code?.Trim();
        var result = await types.ExecuteAsync(new GetPublicPropertyTypesRequest(), context.HttpContext.RequestAborted);
        if (result.IsFailure)
            context.ModelState.AddModelError(string.Empty, result.GetError().Message);
        else
            filters.PropertyTypes = mapper.Map<IReadOnlyList<SelectListItemViewModel>>(result.GetValue());

        if (filters.PropertyTypeId.HasValue && !filters.PropertyTypes.Any(type => type.Id == filters.PropertyTypeId))
            context.ModelState.AddModelError(nameof(filters.PropertyTypeId), "El tipo de propiedad seleccionado no existe.");

        await next();
    }
}
