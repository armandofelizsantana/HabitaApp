using FluentValidation;
using Habita.Application.Dtos.Catalog.Requests;

namespace Habita.Application.Validators.Catalog;

public sealed class GetAllPropertyTypesRequestValidator
    : AbstractValidator<GetAllPropertyTypesRequest>
{
    public GetAllPropertyTypesRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
