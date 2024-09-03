using MimeKit;

namespace AlphaVisa.Infrastructure.Email;
public class Message
{
    public List<MailboxAddress> To { get; set; }

    public string Subject { get; set; }

    public string Content { get; set; }

    public Message(IEnumerable<string> to, string subject, string content)
    {
        To = new List<MailboxAddress>();
        // Assuming that the input is just email addresses without display names
        To.AddRange(to.Select(x => new MailboxAddress(string.Empty, x)));

        Subject = subject;
        Content = content;
    }
}
