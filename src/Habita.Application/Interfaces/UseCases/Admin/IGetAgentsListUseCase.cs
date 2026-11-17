using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Admin.Requests;
using Habita.Application.Dtos.Admin.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.UseCases.Admin;

public interface IGetAgentsListUseCase
    : IUseCase<GetAgentsListRequest, PagedResult<AgentListItemResponse>>;
