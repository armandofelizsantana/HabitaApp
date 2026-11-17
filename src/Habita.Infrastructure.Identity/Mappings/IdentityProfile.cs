using AutoMapper;
using Habita.Application.Dtos.Agent.Responses;
using Habita.Application.Dtos.Auth;
using Habita.Application.Dtos.Client.Responses;
using Habita.Infrastructure.Identity.Entities;

namespace Habita.Infrastructure.Identity.Mappings;

             
                                                                
              
public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<AppUser, RegisterResponseDto>();
        CreateMap<AppUser, ClientProfileResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<AppUser, AgentProfileResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}
