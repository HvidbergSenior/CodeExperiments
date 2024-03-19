namespace Insight.Services.EmailSender.Service;

public interface IEmailConfiguration
{
    public const string DefaultConfigKey = "EmailConfiguration";

    public bool UseEmailConfiguration { get; }
    public string? SmtpServer { get; }
    public int Port { get; }
    public string? Password { get; }
    public string? Username { get; }
    public bool UseSsl { get; }
}

public class EmailConfiguration : IEmailConfiguration
{
    public bool UseEmailConfiguration { get; set; }
    public string? SmtpServer { get; set; }
    public int Port { get; set; }
    public string? Password { get; set; }
    public string? Username { get; set; }
    public bool UseSsl { get; set; }
}