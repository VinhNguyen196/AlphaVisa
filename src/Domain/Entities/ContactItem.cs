using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaVisa.Domain.Entities;
public class ContactItem : BaseAuditableEntity<Guid>
{
    public string? Name { get; set; }

    public string? Thumbnail { get; set; }

    public string? Story { get; set; }

    public string? Metadata { get; set; }
}
