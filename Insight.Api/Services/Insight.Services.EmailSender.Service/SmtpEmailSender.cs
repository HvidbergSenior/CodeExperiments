using System.Net;
using System.Net.Mail;
using JasperFx.Core;
using Microsoft.Extensions.Logging;

namespace Insight.Services.EmailSender.Service;

public class SmtpEmailSender : IEmailSender
{
    private readonly IEmailConfiguration emailConfiguration;
    private readonly ILogger<SmtpEmailSender> logger;

    public SmtpEmailSender(IEmailConfiguration emailConfiguration, ILogger<SmtpEmailSender> logger)
    {
        this.emailConfiguration = emailConfiguration;
        this.logger = logger;
    }


    public async Task<bool> SendEmailAsync(EmailMessage emailMessage)
    {
        var x = emailMessage.To.Join(",");
        using (var message = new MailMessage())
        {
            foreach (var messageTo in emailMessage.To)
            {
                message.To.Add(new MailAddress(messageTo));
            }
            message.From = new MailAddress(emailMessage.From);
            message.Subject = emailMessage.Subject;
            message.Body = emailMessage.Content;
            message.IsBodyHtml = true;
            using (var client = new SmtpClient(emailConfiguration.SmtpServer))
            {
                client.UseDefaultCredentials = false;
                client.Port = emailConfiguration.Port;
                client.Credentials = new NetworkCredential(emailConfiguration.Username, emailConfiguration.Password);
                client.EnableSsl = emailConfiguration.UseSsl;
                try
                {
                    await client.SendMailAsync(message); // Email sent
                    return true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error sending e-mail");
                    return false;
                }
            }
        }
    }
}