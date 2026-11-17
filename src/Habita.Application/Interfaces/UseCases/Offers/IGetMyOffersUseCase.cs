using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Offers.Requests;
using Habita.Application.Dtos.Offers.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.UseCases.Offers;

public interface IGetMyOffersUseCase : IUseCase<GetMyOffersRequest, PagedResult<OfferResponse>>;
