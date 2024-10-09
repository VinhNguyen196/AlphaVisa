using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.ContactItems.Commands;
public record CreateContactItem(string? Name, Guid? ThumbnailId, string? Story, ComplextObject? Metadata) : IRequest<Guid>;

public class CreateContactValidator : AbstractValidator<CreateContactItem>
{
    private readonly IApplicationDbContext _context;

    public CreateContactValidator(IApplicationDbContext context, ISharedLocalizer localizer)
    {
        _context = context;

        RuleFor(n => n.Name)
            .NotEmpty()
            .MustAsync(BeUniqueName)
                .WithMessage(localizer["PropertyMustUnique"])
                .WithErrorCode("Unique");

        RuleFor(n => n.Metadata)
            .NotEmpty();
    }

    private async Task<bool> BeUniqueName(string? name, CancellationToken cancellationToken)
    {
        return await _context.ContactItems
            .AllAsync(x => x.Name != name, cancellationToken);
    }
}

public class CreateContactHandler : IRequestHandler<CreateContactItem, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateContactHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateContactItem request, CancellationToken cancellationToken)
    {
        var entity = new ContactItem
        {
            Name = request?.Name,
            AttachmentItemId = request?.ThumbnailId,
            Story = request?.Story,
            Metadata = request?.Metadata,
        };

        _context.ContactItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
