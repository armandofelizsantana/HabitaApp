using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class ToggleAdminActiveRequestValidator : AbstractValidator<ToggleAdminActiveRequest>
{
    public ToggleAdminActiveRequestValidator()
    {
        RuleFor(x => x.AdminId).NotEmpty().WithMessage("El administrador es requerido.");
    }
}
