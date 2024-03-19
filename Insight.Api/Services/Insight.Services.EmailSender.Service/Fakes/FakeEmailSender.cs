using Insight.Services.EmailSender.Service.Fakes;

namespace Insight.Services.EmailSender.Service;

class FakeEmailSender : IEmailSender
{
    private readonly IFakeEmailOutbox outbox;

    public Task<bool> SendEmailAsync(EmailMessage emailMessage)
    {
        this.outbox.AddMessage(emailMessage);

        var messageSummary = "Sending e-mail:\n\n";
        messageSummary += "To: " + emailMessage.To.ToString();
        messageSummary += $"From: {emailMessage.From}";
        messageSummary += $"Subject: {emailMessage.Subject}";
        messageSummary += $"Content: {emailMessage.Content}";
        Console.WriteLine(messageSummary);
        
        return Task.FromResult(true);
    }

    public FakeEmailSender(IFakeEmailOutbox outbox)
    {
        this.outbox = outbox;
    }
}