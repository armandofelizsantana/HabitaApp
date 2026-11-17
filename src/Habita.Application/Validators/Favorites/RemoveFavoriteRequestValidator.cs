using FluentValidation;
using Habita.Application.Dtos.Favorites.Requests;

namespace Habita.Application.Validators.Favorites;

public sealed class RemoveFavoriteRequestValidator : AbstractValidator<RemoveFavoriteRequest>
{
    public RemoveFavoriteRequestValidator()
    {
        RuleFor(x => x.PropertyId).GreaterThan(0).WithMessage("El ID de propiedad es requerido.");
    }
}
