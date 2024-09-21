namespace AlphaVisa.Domain.Entities;
public class ServiceItem : BaseAuditableEntity<Guid>
{
    public string? Name { get; set; }

    public ServiceItemType Type { get; set; }

    public string? Thumbnail { get; set; }

    public string? Description { get; set; }
}
