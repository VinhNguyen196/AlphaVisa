namespace AlphaVisa.Domain.Entities;
public class NewItem : BaseAuditableEntity<Guid>
{
    public string? Topic { get; set; }

    public string? Thumbnail { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public string? Content { get; set; }
}
