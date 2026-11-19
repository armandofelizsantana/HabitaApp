using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class ToggleClientActiveRequestValidator
    : AbstractValidator<ToggleClientActiveRequest>
{
    public ToggleClientActiveRequestValidator()
    {
        RuleFor(request => request.ClientId).NotEmpty();
    }
}
