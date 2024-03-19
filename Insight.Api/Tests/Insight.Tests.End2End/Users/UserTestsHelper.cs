using System.Web;
using Insight.Services.EmailSender.Service;
using Insight.UserAccess.Application;
using Insight.UserAccess.Domain.User;

namespace Insight.Tests.End2End.Users;

public class UserTestsHelper
{
    public static (string userName, string token) GetUserNameAndTokenFromMessage(EmailMessage message)
    {
        var startUrlWithUsername = "?username=";
        var startToken = "&token=";

        var userName = HttpUtility.UrlDecode(((message.Content.Split(startUrlWithUsername))[1]).Split(startToken)[0]);
        var token = HttpUtility.UrlDecode((message.Content.Split(startToken)[1]).Split("\">")[0].Trim());

        return (userName, token);
    }
}