using FluentValidation;
using Habita.Application.Dtos.Catalog.Requests;

namespace Habita.Application.Validators.Catalog;

public sealed class GetImprovementByIdRequestValidator
    : AbstractValidator<GetImprovementByIdRequest>
{
    public GetImprovementByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El ID de la mejora debe ser mayor que cero.");
    }
}
