using AutoMapper;
using Habita.Application.Dtos.Chat.Responses;
using Habita.Domain.Entities;
using Habita.Domain.Enums;

namespace Habita.Application.Mappings;

public sealed class ChatMappingProfile : Profile
{
    public ChatMappingProfile()
    {
        CreateMap<Message, MessageResponse>()
            .ForMember(d => d.PropertyCode, o => o.MapFrom(s => s.Property.Code.Value))
            .ForMember(
                d => d.SenderId,
                o => o.MapFrom(s => s.SenderType == SenderType.Client ? s.ClientId : s.AgentId)
            )
            .ForMember(d => d.SentAt, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.SenderName, o => o.Ignore());

        CreateMap<Message, ConversationSummaryResponse>()
            .ForMember(d => d.PropertyCode, o => o.MapFrom(s => s.Property.Code.Value))
            .ForMember(d => d.PropertyTitle, o => o.MapFrom(s => s.Property.Title))
            .ForMember(d => d.PropertyDescription, o => o.MapFrom(s => s.Property.Description))
            .ForMember(d => d.LastMessageContent, o => o.MapFrom(s => s.Content))
            .ForMember(d => d.LastMessageSenderType, o => o.MapFrom(s => s.SenderType.ToString()))
            .ForMember(d => d.LastMessageAt, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.OtherUserId, o => o.Ignore())
            .ForMember(d => d.OtherUserName, o => o.Ignore())
            .ForMember(d => d.OtherUserRole, o => o.Ignore());
    }
}
