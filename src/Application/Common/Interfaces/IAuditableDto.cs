namespace AlphaVisa.Application.Common.Interfaces;
public interface IAuditableDto
{
    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
