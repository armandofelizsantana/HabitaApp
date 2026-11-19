using FluentValidation;
using Habita.Application.Dtos.Chat.Requests;

namespace Habita.Application.Validators.Chat;

public sealed class GetMyConversationsRequestValidator
    : AbstractValidator<GetMyConversationsRequest>
{
    public GetMyConversationsRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
        When(x => x.PropertyId.HasValue, () => RuleFor(x => x.PropertyId).GreaterThan(0));
    }
}
