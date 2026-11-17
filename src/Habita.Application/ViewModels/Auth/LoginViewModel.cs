using System.ComponentModel.DataAnnotations;

namespace Habita.Application.ViewModels.Auth;

using Habita.Application.ViewModels.Shared;

public sealed class LoginViewModel : BaseViewModel
{
    [Required(ErrorMessage = "Debe ingresar su correo o nombre de usuario.")]
    [Display(Name = "Correo o nombre de usuario")]
    public string UserNameOrEmail { get; set; } = null!;

    [Required(ErrorMessage = "Debe ingresar su contraseña.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = null!;

    [Display(Name = "Mantener sesión iniciada")]
    public bool RememberMe { get; set; }
}
