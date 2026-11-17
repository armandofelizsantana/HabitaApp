using FluentValidation;
using Habita.Application.Dtos.Offers.Requests;

namespace Habita.Application.Validators.Offers;

public sealed class RejectOfferRequestValidator : AbstractValidator<RejectOfferRequest>
{
    public RejectOfferRequestValidator()
    {
        RuleFor(x => x.OfferId).GreaterThan(0).WithMessage("El ID de oferta es requerido.");
    }
}
