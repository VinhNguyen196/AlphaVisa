using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaVisa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlphaVisa.Infrastructure.Data.Configurations;
public class ContactItemConfiguration : IEntityTypeConfiguration<ContactItem>
{
    public void Configure(EntityTypeBuilder<ContactItem> builder)
    {
    }
}
