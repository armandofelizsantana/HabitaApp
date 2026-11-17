using FluentValidation;
using Habita.Application.Dtos.Property.Requests;

namespace Habita.Application.Validators.Property;

public sealed class DeletePropertyRequestValidator : AbstractValidator<DeletePropertyRequest>
{
    public DeletePropertyRequestValidator()
    {
        RuleFor(x => x.PropertyId).GreaterThan(0).WithMessage("El ID de propiedad es requerido.");
    }
}
