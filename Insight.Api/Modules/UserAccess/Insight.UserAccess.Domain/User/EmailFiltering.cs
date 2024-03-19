using Insight.BuildingBlocks.Domain;
using System.Net.Mail;

namespace Insight.UserAccess.Domain.User
{
    public sealed class EmailFiltering : ValueObject
    {
        public string Value { get; private set; }

        private EmailFiltering()
        {
            Value = string.Empty;
        }

        private EmailFiltering(string value)
        {
         
            Value = value;
        }

        public static EmailFiltering Create(string email)
        {
            return new EmailFiltering(email);
        }

        public static EmailFiltering Empty()
        {
            return new EmailFiltering();
        }

    }
}
