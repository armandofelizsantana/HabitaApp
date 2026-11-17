using FluentValidation;
using Habita.Application.Dtos.Catalog.Requests;

namespace Habita.Application.Validators.Catalog;

public sealed class DeleteSaleTypeRequestValidator : AbstractValidator<DeleteSaleTypeRequest>
{
    public DeleteSaleTypeRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID es requerido.");
    }
}
