using AlphaVisa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlphaVisa.Infrastructure.Data.Configurations;
public class NewItemConfiguration : IEntityTypeConfiguration<NewItem>
{
    public void Configure(EntityTypeBuilder<NewItem> builder)
    {
    }
}
