using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.NewItems.Commands;
public record CreateNewItem(string? Topic, string? Thumbnail, string? Image, string? Description, ComplextObject? Content) : IRequest<Guid>;

public class CreateNewValidator : AbstractValidator<CreateNewItem>
{
    private readonly IApplicationDbContext _context;

    public CreateNewValidator(IApplicationDbContext context, ISharedLocalizer localizer)
    {
        _context = context;

        RuleFor(n => n.Topic)
            .NotEmpty()
            .MustAsync(BeUniqueTopic)
                .WithMessage(localizer["PropertyMustUnique"])
                .WithErrorCode("Unique");

        RuleFor(n => n.Content)
            .NotEmpty();
    }

    private async Task<bool> BeUniqueTopic(string? topic, CancellationToken cancellationToken)
    {
        return await _context.NewItems
            .AllAsync(x => x.Topic != topic, cancellationToken);
    }
}

public class CreateNewHandler : IRequestHandler<CreateNewItem, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateNewHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateNewItem request, CancellationToken cancellationToken)
    {
        var entity = new NewItem
        {
            Topic = request.Topic,
            Thumbnail = request.Thumbnail,
            Image = request.Image,
            Description = request.Description,
            Content = request.Content,
        };

        _context.NewItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
