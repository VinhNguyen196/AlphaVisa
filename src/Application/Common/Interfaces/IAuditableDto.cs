using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaVisa.Application.Common.Interfaces;
public interface IAuditableDto
{
    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
