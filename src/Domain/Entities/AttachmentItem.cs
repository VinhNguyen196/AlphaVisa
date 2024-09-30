namespace AlphaVisa.Domain.Entities;
public class AttachmentItem : BaseAuditableEntity<Guid>
{
    public string? Name { get; set; }

    public string? NameWithoutExtension { get; set; }

    public string? MimeType { get; set; }

    public Guid? ObjectId { get; set; }

    public string? Url { get; set; }

    public string? Encoding { get; set; }

    public long? SizeBytes { get; set; }
}
