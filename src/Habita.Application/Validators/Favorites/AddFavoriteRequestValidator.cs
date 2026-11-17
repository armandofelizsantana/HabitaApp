using FluentValidation;
using Habita.Application.Dtos.Favorites.Requests;

namespace Habita.Application.Validators.Favorites;

public sealed class AddFavoriteRequestValidator : AbstractValidator<AddFavoriteRequest>
{
    public AddFavoriteRequestValidator()
    {
        RuleFor(x => x.PropertyId).GreaterThan(0).WithMessage("El ID de propiedad es requerido.");
    }
}
