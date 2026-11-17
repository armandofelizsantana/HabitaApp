using FluentValidation;
using Habita.Application.Dtos.Auth.Requests;

namespace Habita.Application.Validators.Auth;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty()
            .WithMessage("Debe ingresar su correo o nombre de usuario.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Debe ingresar su contraseña.");
    }
}
