using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Domain
{
    public sealed class CustomerPermissionGroup : ValueObject
    {
        public CustomerId CustomerId { get; private set; }
        public CustomerNumber CustomerNumber { get; private set; }
        public CustomerName CustomerName { get; private set; }
        public List<CustomerPermission> Permissions { get; private set; }

        private CustomerPermissionGroup()
        {
            CustomerId = CustomerId.Empty();
            CustomerNumber = CustomerNumber.Empty();
            CustomerName = CustomerName.Empty();
            Permissions = new List<CustomerPermission>();
        }

        private CustomerPermissionGroup(CustomerId customerId, CustomerNumber customerNumber, CustomerName customerName, IEnumerable<CustomerPermission> permissions)
        {
            CustomerId = customerId;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
            Permissions = permissions.Distinct().ToList();
        }

        public static CustomerPermissionGroup Create(CustomerId customerId, CustomerNumber customerNumber, CustomerName customerName, IEnumerable<CustomerPermission> permissions)
        {
            return new CustomerPermissionGroup(customerId, customerNumber, customerName, permissions);
        }

        public static CustomerPermissionGroup Empty()
        {
            return new CustomerPermissionGroup();
        }
    }
}