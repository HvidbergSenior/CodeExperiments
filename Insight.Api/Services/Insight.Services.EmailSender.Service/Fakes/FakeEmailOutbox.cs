namespace Insight.Services.EmailSender.Service.Fakes;

class FakeEmailOutbox : IFakeEmailOutbox
{
    private List<EmailMessage> messages = new();


    public void AddMessage(EmailMessage message)
    {
        messages.Add(message);
    }

    public IReadOnlyList<EmailMessage> GetAllMessages()
    {
        return messages.AsReadOnly();
    }
}