using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Chat.Requests;
using Habita.Application.Dtos.Chat.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Chat;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Chat;

public sealed class SendMessageUseCase : ISendMessageUseCase
{
    private readonly IMessageRepository _messageRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private readonly IValidator<SendMessageRequest> _validator;
    private readonly ILogger<SendMessageUseCase> _logger;

    public SendMessageUseCase(
        IMessageRepository messageRepository,
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IMapper mapper,
        IValidator<SendMessageRequest> validator,
        ILogger<SendMessageUseCase> logger
    )
    {
        _messageRepository = messageRepository;
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<MessageResponse>> ExecuteAsync(
        SendMessageRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<MessageResponse>();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result<MessageResponse>.Failure(
                Error.Unauthorized(
                    "Auth.NotAuthenticated",
                    "Debe iniciar sesión para enviar un mensaje."
                )
            );

        if (!_currentUser.IsInRole(nameof(Roles.Client)))
            return Result<MessageResponse>.Failure(
                Error.Forbidden(
                    "Auth.ClientOnly",
                    "Solo los clientes pueden iniciar conversaciones."
                )
            );

        var property = await _propertyRepository.GetByIdAsync(
            request.PropertyId,
            cancellationToken
        );
        if (property is null)
            return Result<MessageResponse>.Failure(
                Error.NotFound("Property.NotFound", "No se encontró la propiedad especificada.")
            );

        if (string.IsNullOrWhiteSpace(request.Content))
            return Result<MessageResponse>.Failure(
                Error.Validation("Message.ContentRequired", "No se pueden enviar mensajes vacíos.")
            );

        var createResult = Message.Create(
            request.PropertyId,
            _currentUser.UserId!,
            property.AgentId,
            SenderType.Client,
            request.Content
        );

        if (createResult.IsFailure)
            return Result<MessageResponse>.Failure(createResult.GetError());

        var message = createResult.GetValue();

        await _messageRepository.AddAsync(message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Mensaje enviado por el cliente {ClientId} para la propiedad {PropertyId} al agente {AgentId}.",
            _currentUser.UserId,
            request.PropertyId,
            property.AgentId
        );

        var response = _mapper.Map<MessageResponse>(message);
        response.SenderName = _currentUser.FullName ?? string.Empty;

        return Result<MessageResponse>.Success(response);
    }
}
