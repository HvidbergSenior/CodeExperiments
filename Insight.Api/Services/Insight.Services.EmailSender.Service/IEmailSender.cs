namespace Insight.Services.EmailSender.Service;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(EmailMessage emailMessage);
}