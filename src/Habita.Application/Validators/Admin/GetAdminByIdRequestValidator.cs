using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class GetAdminByIdRequestValidator : AbstractValidator<GetAdminByIdRequest>
{
    public GetAdminByIdRequestValidator()
    {
        RuleFor(x => x.AdminId).NotEmpty().WithMessage("El ID del administrador es requerido.");
    }
}
