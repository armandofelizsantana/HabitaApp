using Habita.Domain.Common;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;
using Habita.Domain.Interfaces.Services;

namespace Habita.Infrastructure.Persistence.Services;

             
                                                                                   
                                                                                       
              
public sealed class OfferPolicy : IOfferPolicy
{
    private readonly IPropertyRepository _propertyRepo;
    private readonly IOfferRepository _offerRepo;
    private readonly IUserRepository _userRepo;

    public OfferPolicy(
        IPropertyRepository propertyRepo,
        IOfferRepository offerRepo,
        IUserRepository userRepo
    )
    {
        _propertyRepo = propertyRepo;
        _offerRepo = offerRepo;
        _userRepo = userRepo;
    }

    public async Task<Result> CanCreateOfferAsync(
        int propertyId,
        string clientId,
        CancellationToken ct = default
    )
    {
        var property = await _propertyRepo.GetByIdAsync(propertyId, ct);
        if (property is null)
            return Result.Failure(
                Error.NotFound("Offer.PropertyNotFound", "La propiedad no existe.")
            );

        if (property.Status != PropertyStatus.Available)
            return Result.Failure(
                Error.Conflict(
                    "Offer.PropertyNotAvailable",
                    "Esta propiedad no está disponible para ofertas."
                )
            );

        var agent = await _userRepo.GetByIdAsync(property.AgentId, ct);
        if (agent is null || !agent.IsActive)
            return Result.Failure(
                Error.Conflict(
                    "Offer.AgentNotAvailable",
                    "El agente responsable no se encuentra disponible para recibir ofertas."
                )
            );

        bool hasAccepted = await _offerRepo.HasAcceptedOfferAsync(propertyId, ct);
        if (hasAccepted)
            return Result.Failure(
                Error.Conflict(
                    "Offer.PropertyHasAcceptedOffer",
                    "La propiedad ya tiene una oferta aceptada."
                )
            );

        bool hasPending = await _offerRepo.HasPendingOfferAsync(propertyId, clientId, ct);
        if (hasPending)
            return Result.Failure(
                Error.Conflict(
                    "Offer.ClientHasPending",
                    "Ya tienes una oferta pendiente para esta propiedad."
                )
            );

        return Result.Success();
    }

    public async Task<Result> CanAcceptOfferAsync(
        int propertyId,
        int offerId,
        CancellationToken ct = default
    )
    {
        var property = await _propertyRepo.GetByIdAsync(propertyId, ct);
        if (property is null)
            return Result.Failure(
                Error.NotFound("Offer.PropertyNotFound", "La propiedad no existe.")
            );

        if (property.Status != PropertyStatus.Available)
            return Result.Failure(
                Error.Conflict(
                    "Offer.PropertyNotAvailable",
                    "La propiedad no está disponible. Ya tiene una oferta aceptada o fue vendida."
                )
            );

        return Result.Success();
    }
}
