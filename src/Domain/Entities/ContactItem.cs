namespace AlphaVisa.Domain.Entities;
public class ContactItem : BaseAuditableEntity<Guid>
{
    public string? Name { get; set; }

    // Explicit foreign key property
    public Guid? AttachmentItemId { get; set; }

    // Optional navigation property (not required for this approach)
    public AttachmentItem? Thumbnail { get; set; }

    public string? Story { get; set; }

    public string? Content { get; set; }
}
