using Habita.Application.Interfaces.Services;

namespace Habita.Application.Dtos.Client.Requests;

public sealed record UpdateClientProfileRequest(
    string FirstName,
    string LastName,
    string Phone,
    IAppFile? PhotoFile
);
