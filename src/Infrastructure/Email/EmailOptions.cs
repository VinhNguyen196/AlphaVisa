using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AlphaVisa.Infrastructure.Email;
public class EmailOptions
{
    public string? From { get; set; }

    public string? Smtp { get; set; }

    public int? Port { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }
}
