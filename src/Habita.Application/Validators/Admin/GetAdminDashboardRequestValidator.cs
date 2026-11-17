using FluentValidation;
using Habita.Application.Dtos.Admin.Requests;

namespace Habita.Application.Validators.Admin;

public sealed class GetAdminDashboardRequestValidator : AbstractValidator<GetAdminDashboardRequest>
{
    public GetAdminDashboardRequestValidator() { }
}
