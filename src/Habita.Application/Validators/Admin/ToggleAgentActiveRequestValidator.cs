using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class ToggleAgentActiveRequestValidator : AbstractValidator<ToggleAgentActiveRequest>
{
    public ToggleAgentActiveRequestValidator()
    {
        RuleFor(x => x.AgentId).NotEmpty().WithMessage("El agente es requerido.");
    }
}
