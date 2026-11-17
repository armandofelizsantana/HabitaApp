using Habita.Application.Interfaces.Services;

namespace Habita.Application.Dtos.Agent.Requests;

public sealed record UpdateAgentProfileRequest(
    string FirstName,
    string LastName,
    string Phone,
    IAppFile? PhotoFile
);
