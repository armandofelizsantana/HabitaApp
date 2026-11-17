using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class ToggleDeveloperActiveRequestValidator
    : AbstractValidator<ToggleDeveloperActiveRequest>
{
    public ToggleDeveloperActiveRequestValidator()
    {
        RuleFor(x => x.DeveloperId).NotEmpty().WithMessage("El desarrollador es requerido.");
    }
}
