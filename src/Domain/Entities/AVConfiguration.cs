using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaVisa.Domain.Entities;
public class AVConfiguration : BaseAuditableEntity<Guid>
{
    public string? Language { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? AddressLink { get; set; }

    public string? SocialLink { get; set; }
}
