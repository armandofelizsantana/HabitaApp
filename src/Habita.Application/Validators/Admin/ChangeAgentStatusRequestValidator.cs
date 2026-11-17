using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class ChangeAgentStatusRequestValidator : AbstractValidator<ChangeAgentStatusRequest>
{
    public ChangeAgentStatusRequestValidator()
    {
        RuleFor(x => x.AgentId).NotEmpty().WithMessage("El agente es requerido.");
        RuleFor(x => x.Status).NotNull().WithMessage("El estado es requerido.");
    }
}
