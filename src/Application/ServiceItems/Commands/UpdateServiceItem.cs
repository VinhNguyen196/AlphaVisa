using AlphaVisa.Application.Common.Caches;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;
using AlphaVisa.Domain.Enums;

namespace AlphaVisa.Application.ServiceItems.Commands;
public class UpdateServiceItemCommand : EntityCacheBase, IRequest
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public ServiceItemType? Type { get; set; }

    public string? Thumbnail { get; set; }

    public ComplextObject? Description { get; set; }

}

public class UpdateServiceItemCommandValidator : AbstractValidator<UpdateServiceItemCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateServiceItemCommandValidator(IApplicationDbContext context, ISharedLocalizer localizer)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;
        _context = context;

        RuleFor(si => si.Id)
           .NotEmpty()
           .MustAsync(async (si, id, cancellationToken) =>
           {
               var entity = await _context.ServiceItems
                   .SingleOrDefaultAsync(x => x.Id == si.Id, cancellationToken);

               if (entity == null)
               {
                   return false;
               }

               // Store the entity in the command context
               si.SetEntity(entity);

               return true;
           })
           .WithMessage(localizer["PropertyMustExist"])
           .WithErrorCode("NotExist");

        RuleFor(si => si.Name)
            .NotEmpty()
            .MustAsync((si, name, clt) => BeUniqueName(si.Id, name, clt))
                .WithMessage(localizer["PropertyMustUnique"])
                .WithErrorCode("Unique");

        RuleFor(si => si.Type)
            .NotEmpty()
            .IsInEnum();

        RuleFor(si => si.Description)
            .NotEmpty();
    }

    public async Task<bool> BeExist(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ServiceItems
                .AnyAsync(si => si.Id == id, cancellationToken);
    }

    public async Task<bool> BeUniqueName(Guid id, string? name, CancellationToken cancellationToken)
    {
        return await _context.ServiceItems
            .Where(x => x.Id != id)
            .AllAsync(x => x.Name != name, cancellationToken);
    }
}

public class UpdateServiceItemCommandHandler : IRequestHandler<UpdateServiceItemCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateServiceItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateServiceItemCommand request, CancellationToken cancellationToken)
    {
        var entity = request.GetEntity<ServiceItem>();

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Type = request.Type.GetValueOrDefault();
        entity.Thumbnail = request.Thumbnail;
        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
