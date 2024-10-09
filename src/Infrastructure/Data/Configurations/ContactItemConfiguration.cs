using AlphaVisa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlphaVisa.Infrastructure.Data.Configurations;
public class ContactItemConfiguration : IEntityTypeConfiguration<ContactItem>
{
    public void Configure(EntityTypeBuilder<ContactItem> builder)
    {
        builder.HasOne(ci => ci.Thumbnail)
            .WithOne()
            .HasForeignKey<ContactItem>(ci => ci.AttachmentItemId)
            .IsRequired(false);
    }
}
