using Microsoft.AspNetCore.Mvc;
using Habita.Application.Interfaces.Services;

namespace Habita.WebApp.Controllers;

public abstract class BaseController : Controller
{
    protected BaseController(ICurrentUserService currentUser)
    {
        CurrentUser = currentUser;
    }

    protected ICurrentUserService CurrentUser { get; }
}
