using AutoMapper;
using Habita.Application.Dtos.Admin.Requests;
using Habita.Application.Dtos.Admin.Responses;
using Habita.Application.Dtos.Agent.Requests;
using Habita.Application.Dtos.Agent.Responses;
using Habita.Application.Dtos.Auth.Requests;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Chat.Responses;
using Habita.Application.Dtos.Client.Requests;
using Habita.Application.Dtos.Client.Responses;
using Habita.Application.Dtos.Favorites.Responses;
using Habita.Application.Dtos.Offers.Responses;
using Habita.Application.Dtos.Property.Requests;
using Habita.Application.Dtos.Property.Responses;
using Habita.Application.ViewModels.Admin;
using Habita.Application.ViewModels.Agent;
using Habita.Application.ViewModels.Auth;
using Habita.Application.ViewModels.Chat;
using Habita.Application.ViewModels.Client;
using Habita.Application.ViewModels.Offers;
using Habita.Application.ViewModels.Property;
using Habita.Application.ViewModels.Home;
using Habita.Application.ViewModels.Shared;

namespace Habita.Application.Mappings;

public sealed class ViewModelMappingProfile : Profile
{
    public ViewModelMappingProfile()
    {
        MapAuth();
        MapProperty();
        MapClient();
        MapAgent();
        MapAdmin();
        MapCatalog();
        MapChat();
        MapOffers();
        MapShared();
    }

    private void MapAuth()
    {
        CreateMap<LoginViewModel, LoginRequest>();
        CreateMap<ForgotPasswordViewModel, ForgotPasswordRequest>()
            .ForMember(d => d.Origin, opt => opt.Ignore());
        CreateMap<ResetPasswordViewModel, ResetPasswordRequest>();
        CreateMap<ChangePasswordViewModel, ChangePasswordRequest>();
    }

    private void MapProperty()
    {
        CreateMap<PropertyListItemResponse, PropertyListItemViewModel>();
        CreateMap<PropertyDetailResponse, PropertyDetailPublicViewModel>();
        CreateMap<PropertyFilterViewModel, GetPropertyListRequest>();
        CreateMap<PropertySearchByCodeViewModel, SearchPropertyByCodeRequest>();
    }

    private void MapClient()
    {
        CreateMap<ClientDashboardResponse, ClientDashboardViewModel>();
        CreateMap<ClientProfileResponse, ClientProfileViewModel>();
        CreateMap<UpdateClientProfileViewModel, UpdateClientProfileRequest>();
        CreateMap<FavoriteResponse, FavoriteListItemViewModel>();

        CreateMap<PropertyListItemResponse, PropertyListItemViewModel>();
    }

    private void MapAgent()
    {
        CreateMap<PublicAgentResponse, PublicAgentListItemViewModel>();
        CreateMap<PropertyListItemResponse, AgentPropertyListItemViewModel>();
        CreateMap<PropertySummaryResponse, AgentPropertyListItemViewModel>();
        CreateMap<AgentProfileResponse, AgentProfileViewModel>();
        CreateMap<UpdateAgentProfileViewModel, UpdateAgentProfileRequest>();

        CreateMap<CreatePropertyViewModel, CreatePropertyRequest>()
            .ForMember(d => d.ImageFiles, opt => opt.Ignore())
            .ForMember(d => d.ImprovementIds, opt => opt.MapFrom(s => s.ImprovementIds));

        CreateMap<EditPropertyViewModel, UpdatePropertyRequest>()
            .ForMember(d => d.PropertyId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.NewImageFiles, opt => opt.Ignore())
            .ForMember(d => d.ImageIdsToRemove, opt => opt.Ignore())
            .ForMember(d => d.ImprovementIdsToAdd, opt => opt.MapFrom(s => s.ImprovementIds))
            .ForMember(d => d.ImprovementIdsToRemove, opt => opt.Ignore());

        CreateMap<PropertyDetailResponse, AgentPropertyDetailViewModel>();

        CreateMap<SendMessageViewModel, SendMessageRequest>()
            .ForMember(d => d.PropertyId, opt => opt.Ignore())
            .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Message));

        CreateMap<ReplyMessageViewModel, ReplyMessageRequest>()
            .ForMember(d => d.MessageId, opt => opt.Ignore())
            .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Message));
    }

    private void MapAdmin()
    {
        CreateMap<AdminDashboardResponse, AdminDashboardViewModel>();
        CreateMap<AgentListItemResponse, AgentListItemViewModel>();
        CreateMap<AdminListItemResponse, AdminListItemViewModel>();
        CreateMap<DeveloperListItemResponse, DeveloperListItemViewModel>();

        CreateMap<ToggleAgentStatusViewModel, ToggleAgentActiveRequest>();
        CreateMap<DeleteAgentViewModel, DeleteAgentRequest>();
        CreateMap<CreateAdminViewModel, CreateAdminRequest>();
        CreateMap<EditAdminViewModel, UpdateAdminRequest>()
            .ConstructUsing(s => new UpdateAdminRequest(
                s.Id, s.FirstName, s.LastName,
                s.IdentityDocument, s.Email, s.UserName,
                s.NewPassword, s.ConfirmPassword));
        CreateMap<ToggleAdminStatusViewModel, ToggleAdminActiveRequest>();
        CreateMap<CreateDeveloperViewModel, CreateDeveloperRequest>();
        CreateMap<EditDeveloperViewModel, UpdateDeveloperRequest>()
            .ConstructUsing(s => new UpdateDeveloperRequest(
                s.Id, s.FirstName, s.LastName,
                s.IdentityDocument, s.Email, s.UserName,
                s.NewPassword, s.ConfirmPassword));
        CreateMap<ToggleDeveloperStatusViewModel, ToggleDeveloperActiveRequest>();
    }

    private void MapCatalog()
    {
        CreateMap<PropertyTypeResponse, PropertyTypeListItemViewModel>();
        CreateMap<CreatePropertyTypeViewModel, CreatePropertyTypeRequest>();
        CreateMap<EditPropertyTypeViewModel, UpdatePropertyTypeRequest>();

        CreateMap<SaleTypeResponse, SaleTypeListItemViewModel>();
        CreateMap<CreateSaleTypeViewModel, CreateSaleTypeRequest>();
        CreateMap<EditSaleTypeViewModel, UpdateSaleTypeRequest>();

        CreateMap<ImprovementResponse, ImprovementListItemViewModel>();
        CreateMap<CreateImprovementViewModel, CreateImprovementRequest>();
        CreateMap<EditImprovementViewModel, UpdateImprovementRequest>();
    }

    private void MapChat()
    {
        CreateMap<ConversationSummaryResponse, ConversationSummaryViewModel>();
        CreateMap<MessageResponse, MessageViewModel>()
            .ForMember(d => d.IsFromCurrentUser, o => o.Ignore());
    }

    private void MapOffers()
    {
        CreateMap<OfferResponse, OfferListItemViewModel>();
        CreateMap<OfferClientSummaryResponse, OfferClientSummaryViewModel>();
        CreateMap<OfferResponse, ViewModels.Offers.OfferDetailViewModel>();
    }

    private void MapShared()
    {
        CreateMap<PropertyTypeResponse, SelectListItemViewModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Selected, opt => opt.MapFrom(_ => false));

        CreateMap<SaleTypeResponse, SelectListItemViewModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Selected, opt => opt.MapFrom(_ => false));

        CreateMap<ImprovementResponse, SelectListItemViewModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Selected, opt => opt.MapFrom(_ => false));
    }
}
