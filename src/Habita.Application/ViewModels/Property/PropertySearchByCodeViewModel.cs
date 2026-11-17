using System.ComponentModel.DataAnnotations;

namespace Habita.Application.ViewModels.Property;

using Habita.Application.ViewModels.Shared;

public sealed class PropertySearchByCodeViewModel : BaseViewModel
{
    [Display(Name = "Código de propiedad")]
    [Required(ErrorMessage = "El código de propiedad es requerido.")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "El código de propiedad debe ser de 6 dígitos.")]
    public string Code { get; set; } = null!;
}
