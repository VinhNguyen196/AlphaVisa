namespace AlphaVisa.Domain.Entities;
public class NewItem : BaseAuditableEntity<Guid>
{
    public string? Topic { get; set; }

   // Explicit foreign key property
    public Guid? AttachmentItemId { get; set; }

    // Optional navigation property (not required for this approach)
    public AttachmentItem? Thumbnail { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public string? Content { get; set; }
}
