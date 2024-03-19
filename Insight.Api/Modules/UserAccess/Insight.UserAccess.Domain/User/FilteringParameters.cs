using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class FilteringParameters : ValueObject
    {
        public EmailFiltering EmailFiltering { get; private set; } = EmailFiltering.Empty();
        public UserStatus UserStatus { get; private set; }
        public CustomerName CustomerName { get; private set; } = CustomerName.Empty();
        public CustomerNumber CustomerNumber { get; private set; } = CustomerNumber.Empty();

        private FilteringParameters()
        {
            //Left empty for serialization purposes
        }

        private FilteringParameters(EmailFiltering emailFiltering, UserStatus userStatus, CustomerName customerName, CustomerNumber customerNumber)
        {
            EmailFiltering = emailFiltering;
            UserStatus = userStatus;
            CustomerName = customerName;
            CustomerNumber = customerNumber;
        }

        public static FilteringParameters Create(EmailFiltering emailFiltering, UserStatus userStatus, CustomerName customerName, CustomerNumber customerNumber)
        {
            return new FilteringParameters(emailFiltering, userStatus, customerName, customerNumber);
        }

        public static FilteringParameters Empty()
        {
            return new FilteringParameters();
        }
    }
}
