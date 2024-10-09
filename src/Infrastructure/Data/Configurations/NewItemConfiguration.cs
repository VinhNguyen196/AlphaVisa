using AlphaVisa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlphaVisa.Infrastructure.Data.Configurations;
public class NewItemConfiguration : IEntityTypeConfiguration<NewItem>
{
    public void Configure(EntityTypeBuilder<NewItem> builder)
    {
        builder.HasOne(ni => ni.Thumbnail)
            .WithOne()
            .HasForeignKey<NewItem>(ni => ni.AttachmentItemId)
            .IsRequired(false);
    }
}
