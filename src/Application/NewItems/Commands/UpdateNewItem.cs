using AlphaVisa.Application.Common.Caches;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.NewItems.Commands;
public class UpdateNewItem : EntityCacheBase, IRequest
{
    public Guid Id { get; set; }

    public string? Topic { get; set; }

    public Guid? ThumbnailId { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public ComplextObject? Content { get; set; }
}

public class UpdateNewItemValidator : AbstractValidator<UpdateNewItem>
{
    private readonly IApplicationDbContext _context;

    public UpdateNewItemValidator(IApplicationDbContext context, ISharedLocalizer localizer)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;
        _context = context;

        RuleFor(n => n.Id)
            .NotEmpty()
            .MustAsync(async (n, id, clt) =>
            {
                var entity = await _context.NewItems
                    .SingleOrDefaultAsync(x => x.Id == id, clt);

                if (entity == null) return false;

                n.SetEntity(entity);

                return true;
            })
            .WithMessage(localizer["PropertyMustExist"])
            .WithErrorCode("NotExist");

        RuleFor(n => n.Topic)
            .NotEmpty()
            .MustAsync((n, topic, ctl) => BeUniqueTopic(n.Id, topic, ctl))
                .WithMessage(localizer["PropertyMustUnique"])
                .WithErrorCode("Unique");

        RuleFor(n => n.Content)
            .NotEmpty();
    }

    public async Task<bool> BeUniqueTopic(Guid id, string? topic, CancellationToken cancellationToken)
    {
        return await _context.NewItems
            .Where(x => x.Id != id)
            .AllAsync(x => x.Topic != topic, cancellationToken);
    }
}

public class UpdateNewItemHandler : IRequestHandler<UpdateNewItem>
{
    private readonly IApplicationDbContext _context;

    public UpdateNewItemHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateNewItem request, CancellationToken cancellationToken)
    {
        var entity = request.GetEntity<NewItem>();

        Guard.Against.NotFound(request.Id, entity);

        entity.Topic = request.Topic;
        entity.AttachmentItemId = request.ThumbnailId;
        entity.Image = request.Image;
        entity.Description = request.Description;
        entity.Content = request.Content;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
