using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaVisa.Application.Common.Interfaces;
public interface IMailService
{
    public Task SendEmailAsync(IEnumerable<string> to, string subject, string content);

    public Task SendHtmlEmailAsync(IEnumerable<string> to, string subject, string htmlContent);
}
