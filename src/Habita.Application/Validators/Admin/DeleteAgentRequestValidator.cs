using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class DeleteAgentRequestValidator : AbstractValidator<DeleteAgentRequest>
{
    public DeleteAgentRequestValidator()
    {
        RuleFor(x => x.AgentId).NotEmpty().WithMessage("El agente es requerido.");
    }
}
