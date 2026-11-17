using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Agent.Requests;
using Habita.Application.Dtos.Agent.Responses;

namespace Habita.Application.Interfaces.UseCases.Agent;

public interface IGetAgentProfileUseCase : IUseCase<GetAgentProfileRequest, AgentProfileResponse>;
