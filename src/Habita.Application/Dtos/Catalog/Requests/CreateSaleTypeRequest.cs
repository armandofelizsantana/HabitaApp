using Habita.Domain.Enums;

namespace Habita.Application.Dtos.Catalog.Requests;

public sealed record CreateSaleTypeRequest(SaleTypeCode Code, string Name, string Description);
