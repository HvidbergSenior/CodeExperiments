using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.Auth
{
    public sealed class AccessToken : ValueObject
    {
        public string Value { get; private set; }

        private AccessToken()
        {
            Value = String.Empty;
        }

        private AccessToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }
            Value = value;
        }

        public static AccessToken Create(string accessToken)
        {  
            return new AccessToken(accessToken);
        }

        public static AccessToken Empty()
        {
            return new AccessToken();
        }        
    }
}
