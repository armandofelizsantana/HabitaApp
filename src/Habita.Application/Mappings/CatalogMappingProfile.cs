using AutoMapper;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Domain.Entities;

namespace Habita.Application.Mappings;

public sealed class CatalogMappingProfile : Profile
{
    public CatalogMappingProfile()
    {
        CreateMap<Improvement, ImprovementResponse>();

        CreateMap<PropertyType, PropertyTypeResponse>();

        CreateMap<SaleType, SaleTypeResponse>()
            .ConstructUsing(s => new SaleTypeResponse(
                s.Id, s.Code.ToString(), s.Name, s.Description));
    }
}
