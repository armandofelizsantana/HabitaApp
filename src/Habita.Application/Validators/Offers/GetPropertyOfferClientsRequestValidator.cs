using FluentValidation;
using Habita.Application.Dtos.Offers.Requests;

namespace Habita.Application.Validators.Offers;

public sealed class GetPropertyOfferClientsRequestValidator
    : AbstractValidator<GetPropertyOfferClientsRequest>
{
    public GetPropertyOfferClientsRequestValidator()
    {
        RuleFor(request => request.PropertyId).GreaterThan(0);
        RuleFor(request => request.Page).GreaterThan(0);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 50);
    }
}
