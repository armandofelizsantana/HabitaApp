using Habita.Domain.Enums;

namespace Habita.Application.Dtos.Catalog.Requests;

public sealed record UpdateSaleTypeRequest(
    int Id,
    SaleTypeCode Code,
    string Name,
    string Description
);
