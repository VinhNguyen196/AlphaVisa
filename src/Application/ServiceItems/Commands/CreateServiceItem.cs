using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;
using AlphaVisa.Domain.Enums;

namespace AlphaVisa.Application.ServiceItems.Commands;

public record CreateServiceItemCommand : IRequest<Guid>
{
    public string? Name { get; set; }

    public ServiceItemType? Type { get; set; }

    public string? Thumbnail { get; set; }

    public ComplextObject? Description { get; set; }
}

public class CreateServiceItemCommandValidator : AbstractValidator<CreateServiceItemCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateServiceItemCommandValidator(IApplicationDbContext context, ISharedLocalizer localizer)
    {
        _context = context;

        RuleFor(si => si.Name)
            .NotEmpty()
            .MustAsync(BeUniqueName)
                .WithMessage(localizer["PropertyMustUnique"])
                .WithErrorCode("Unique");

        RuleFor(si => si.Type)
            .NotEmpty()
            .IsInEnum();

        RuleFor(si => si.Description)
            .NotEmpty();
    }

    public async Task<bool> BeUniqueName(string? name, CancellationToken cancellationToken)
    {
        return await _context.ServiceItems
            .AllAsync(si => si.Name != name, cancellationToken);
    }
}

public class CreateServiceItemCommandHandler : IRequestHandler<CreateServiceItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateServiceItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateServiceItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new ServiceItem
        {
            Name = request.Name,
            Type = request.Type ?? ServiceItemType.VISIT,
            Thumbnail = request.Thumbnail,
            Description = request.Description,
        };

        _context.ServiceItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
