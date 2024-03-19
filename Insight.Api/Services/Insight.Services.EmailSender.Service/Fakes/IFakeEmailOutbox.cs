namespace Insight.Services.EmailSender.Service.Fakes;

public interface IFakeEmailOutbox
{
    void AddMessage(EmailMessage message);
    IReadOnlyList<EmailMessage> GetAllMessages();
}