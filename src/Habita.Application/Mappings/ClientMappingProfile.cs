using AutoMapper;
using Habita.Application.Dtos.Agent.Responses;
using Habita.Application.Dtos.Client.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Mappings;

public sealed class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<UserInfo, ClientProfileResponse>();
        CreateMap<UserInfo, AgentProfileResponse>();
    }
}
