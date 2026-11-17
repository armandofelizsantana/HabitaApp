using System.ComponentModel.DataAnnotations;

namespace Habita.Application.ViewModels.Agent;

using Habita.Application.ViewModels.Shared;

public sealed class SendMessageViewModel : BaseViewModel
{
    [Required(ErrorMessage = "Debe escribir un mensaje antes de enviarlo.")]
    [Display(Name = "Mensaje")]
    public string Message { get; set; } = null!;
}
