using AlphaVisa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlphaVisa.Infrastructure.Data.Configurations;
public class AVConfigInfoConfiguration : IEntityTypeConfiguration<AVConfiguration>
{
    public void Configure(EntityTypeBuilder<AVConfiguration> builder)
    {
    }
}
