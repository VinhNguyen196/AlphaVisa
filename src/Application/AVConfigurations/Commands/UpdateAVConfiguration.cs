using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.AVConfigurations.Commands;
public record UpdateAVConfigurationCommand(string? Language, 
    string? Phone, 
    string? Email, 
    string? Address, 
    string? AddressLink, 
    string? SocialLink,
    string? AddressMap) 
    : IRequest;

public class UpdateAVConfigurationCommandValidator : AbstractValidator<UpdateAVConfigurationCommand>
{

    public UpdateAVConfigurationCommandValidator(ISharedLocalizer localizer)
    {
        RuleFor(c => c.Phone)
            .Matches(@"^\+?[0-9]{1,4}?[-. ]?(\(?\d{1,4}?\)?[-. ]?)?[\d-. ]{5,10}$")
            .When(c => !string.IsNullOrEmpty(c.Phone))
            .WithMessage(localizer["PhoneInvalidFormat"]);

        RuleFor(c => c.Email)
            .EmailAddress()
            .When(c => !string.IsNullOrEmpty(c.Email))
            .WithMessage(localizer["EmailInvalid"]);
    }
}

public class UpdateAVConfigurationCommandHandler : IRequestHandler<UpdateAVConfigurationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAVConfigurationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAVConfigurationCommand request, CancellationToken cancellationToken)
    {
        var entity = new AVConfiguration()
        {
            Language = request?.Language,
            Phone = request?.Phone,
            Email = request?.Email,
            Address = request?.Address,
            AddressLink = request?.AddressLink,
            SocialLink = request?.SocialLink,
            AddressMap = request?.AddressMap,
        };

        _context.AVConfigurations.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return;
    }
}

