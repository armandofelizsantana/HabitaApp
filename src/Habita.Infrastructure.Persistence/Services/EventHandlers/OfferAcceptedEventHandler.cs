using Microsoft.Extensions.Logging;
using Habita.Domain.Enums;
using Habita.Domain.Events;
using Habita.Domain.Interfaces.Events;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Infrastructure.Persistence.Services.EventHandlers;

             
                                                                            
                                                  
                                                                  
              
public sealed class OfferAcceptedEventHandler : IEventHandler<OfferAcceptedEvent>
{
    private readonly IPropertyRepository _propertyRepo;
    private readonly IOfferRepository _offerRepo;
    private readonly ILogger<OfferAcceptedEventHandler> _logger;

    public OfferAcceptedEventHandler(
        IPropertyRepository propertyRepo,
        IOfferRepository offerRepo,
        ILogger<OfferAcceptedEventHandler> logger
    )
    {
        _propertyRepo = propertyRepo;
        _offerRepo = offerRepo;
        _logger = logger;
    }

    public async Task HandleAsync(OfferAcceptedEvent @event, CancellationToken ct = default)
    {
        var property = await _propertyRepo.GetByIdAsync(@event.PropertyId, ct);
        if (property is null)
        {
            _logger.LogError(
                "OfferAccepted: propiedad {PropertyId} no encontrada.",
                @event.PropertyId
            );
            return;
        }

        property.MarkAsSold();
        _propertyRepo.Update(property);

        var pendingOffers = await _offerRepo.GetByPropertyAsync(@event.PropertyId, ct: ct);
        foreach (var offer in pendingOffers)
        {
            if (offer.Id != @event.OfferId && offer.Status == OfferStatus.Pending)
            {
                offer.Reject();
                _offerRepo.Update(offer);
            }
        }

        _logger.LogInformation(
            "Oferta {OfferId} aceptada. Propiedad {PropertyId} marcada como vendida. "
                + "Ofertas pendientes restantes rechazadas.",
            @event.OfferId,
            @event.PropertyId
        );
    }
}
