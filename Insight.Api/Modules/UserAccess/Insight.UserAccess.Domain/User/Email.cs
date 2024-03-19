using Insight.BuildingBlocks.Domain;
using System.Net.Mail;

namespace Insight.UserAccess.Domain.User
{
    public sealed class Email : ValueObject
    {
        public string Value { get; private set; }

        private Email()
        {
            Value = String.Empty;
        }

        private Email(string value)
        {
            if (!IsValidEmail(value))
            {
                throw new FormatException("Invalid email format");
            }
            Value = value;
        }

        public static Email Create(string email)
        {
            return new Email(email);
        }

        public static Email Empty()
        {
            return new Email();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAdr = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
