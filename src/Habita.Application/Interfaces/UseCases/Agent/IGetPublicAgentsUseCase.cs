using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Agent.Requests;
using Habita.Application.Dtos.Agent.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.UseCases.Agent;

public interface IGetPublicAgentsUseCase
    : IUseCase<GetPublicAgentsRequest, PagedResult<PublicAgentResponse>>;
