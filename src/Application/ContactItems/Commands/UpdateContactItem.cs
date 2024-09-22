using AlphaVisa.Application.Common.Caches;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.ContactItems.Commands;
public class UpdateContactItem : EntityCacheBase, IRequest
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Thumbnail { get; set; }

    public string? Story { get; set; }

    public ComplextObject? Metadata { get; set; }
}

public class UpdateContactItemValidator : AbstractValidator<UpdateContactItem>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactItemValidator(IApplicationDbContext context, ISharedLocalizer localizer)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;
        _context = context;

        RuleFor(n => n.Id)
            .NotEmpty()
            .MustAsync(async (n, id, clt) =>
            {
                var entity = await _context.ContactItems
                    .SingleOrDefaultAsync(x => x.Id == id, clt);

                if (entity == null) return false;

                n.SetEntity(entity);

                return true;
            })
            .WithMessage(localizer["PropertyMustExist"])
            .WithErrorCode("NotExist");

        RuleFor(n => n.Name)
            .NotEmpty()
            .MustAsync((n, name, ctl) => BeUniqueName(n.Id, name, ctl))
                .WithMessage(localizer["PropertyMustUnique"])
                .WithErrorCode("Unique");

        RuleFor(n => n.Metadata)
            .NotEmpty();
    }

    public async Task<bool> BeUniqueName(Guid id, string? name, CancellationToken cancellationToken)
    {
        return await _context.ContactItems
            .Where(x => x.Id != id)
            .AllAsync(x => x.Name != name, cancellationToken);
    }
}

public class UpdateContactItemHandler : IRequestHandler<UpdateContactItem>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactItemHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateContactItem request, CancellationToken cancellationToken)
    {
        var entity = request.GetEntity<ContactItem>();

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Thumbnail = request.Thumbnail;
        entity.Story = request.Story;
        entity.Metadata = request.Metadata;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
