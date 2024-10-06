using AlphaVisa.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AlphaVisa.Infrastructure.Email;
public class MailService : IMailService
{
    private readonly IOptions<EmailOptions> _emailOptions;

    public MailService(IOptions<EmailOptions> options)
    {
        _emailOptions = options;
    }

    public async Task SendAndSavedHtmlEmailAsync(IEnumerable<string> to, string subject, string htmlContent)
    {
        Message message = new Message(to.Append(_emailOptions.Value.From ?? string.Empty), subject, htmlContent);
        var readyMessage = CreateEmailMessage(message);
        readyMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlContent
        };

        await Send(readyMessage);
    }

    public async Task SendEmailAsync(IEnumerable<string> to, string subject, string content)
    {
        Message message = new Message(to, subject, content);
        var readyMessage = CreateEmailMessage(message);
        await Send(readyMessage);
    }

    public async Task SendHtmlEmailAsync(IEnumerable<string> to, string subject, string htmlContent)
    {
        Message message = new Message(to, subject, htmlContent);
        var readyMessage = CreateEmailMessage(message);
        readyMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlContent
        };

        await Send(readyMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(string.Empty, _emailOptions.Value.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;
    }

    private async Task Send(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_emailOptions.Value.Smtp, _emailOptions.Value.Port ?? 465, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailOptions.Value.UserName, _emailOptions.Value.Password);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
