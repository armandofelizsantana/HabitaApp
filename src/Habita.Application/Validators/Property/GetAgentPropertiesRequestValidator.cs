using FluentValidation;
using Habita.Application.Dtos.Property.Requests;

namespace Habita.Application.Validators.Property;

public sealed class GetAgentPropertiesRequestValidator
    : AbstractValidator<GetAgentPropertiesRequest>
{
    public GetAgentPropertiesRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
