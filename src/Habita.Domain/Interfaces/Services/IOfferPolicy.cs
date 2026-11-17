using Habita.Domain.Common;

namespace Habita.Domain.Interfaces.Services;

             
                                                                                             
                                                                                              
                                                                 
                                                                     
              
public interface IOfferPolicy
{
                                                               
    Task<Result> CanCreateOfferAsync(
        int propertyId,
        string clientId,
        CancellationToken ct = default
    );

                                                                                              
    Task<Result> CanAcceptOfferAsync(int propertyId, int offerId, CancellationToken ct = default);
}
