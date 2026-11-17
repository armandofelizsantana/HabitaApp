using FluentValidation;
using Habita.Application.Dtos.Client.Requests;

namespace Habita.Application.Validators.Client;

public sealed class GetClientDashboardRequestValidator
    : AbstractValidator<GetClientDashboardRequest> { }
