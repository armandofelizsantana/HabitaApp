using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Offers.Requests;
using Habita.Application.Dtos.Offers.Responses;

namespace Habita.Application.Interfaces.UseCases.Offers;

public interface ICreateOfferUseCase : IUseCase<CreateOfferRequest, OfferResponse>;
