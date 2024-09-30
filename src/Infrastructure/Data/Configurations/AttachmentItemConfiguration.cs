using AlphaVisa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlphaVisa.Infrastructure.Data.Configurations;
public class AttachmentItemConfiguration : IEntityTypeConfiguration<AttachmentItem>
{
    public void Configure(EntityTypeBuilder<AttachmentItem> builder)
    {
    }
}
