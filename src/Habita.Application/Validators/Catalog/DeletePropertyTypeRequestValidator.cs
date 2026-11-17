using FluentValidation;
using Habita.Application.Dtos.Catalog.Requests;

namespace Habita.Application.Validators.Catalog;

public sealed class DeletePropertyTypeRequestValidator
    : AbstractValidator<DeletePropertyTypeRequest>
{
    public DeletePropertyTypeRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID es requerido.");
    }
}
