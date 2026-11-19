using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class GetClientsListRequestValidator : AbstractValidator<GetClientsListRequest>
{
    public GetClientsListRequestValidator()
    {
        RuleFor(request => request.Page).GreaterThanOrEqualTo(1);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 100);
    }
}
