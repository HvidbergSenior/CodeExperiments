namespace Insight.Services.EmailSender.Service;

public class EmailMessage
{
    public string From { get; set; }
    public List<string> To { get; set; }

    public string Subject { get; set; }
    public string Content { get; set; }

    public EmailMessage(string from, IEnumerable<string> to, string subject, string content)
    {
        From = from;
        To = to.ToList();
        Subject = subject;
        Content = content;
    }
}