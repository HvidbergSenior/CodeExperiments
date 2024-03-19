using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class ReducedCustomerFromRepository : ValueObject
{
    public CustomerId CustomerId { get; private set; }
    public CustomerName CustomerName { get; private set; }
    public CustomerNumber CustomerNumber { get; private set; }
    public CustomerNumber ParentCustomerNumber { get; private set; }

    private ReducedCustomerFromRepository()
    {
        CustomerId = CustomerId.Empty();
        CustomerName = CustomerName.Empty();
        CustomerNumber = CustomerNumber.Empty();
        ParentCustomerNumber = CustomerNumber.Empty();
    }

    private ReducedCustomerFromRepository(CustomerId customerId, CustomerName customerName, CustomerNumber customerNumber, CustomerNumber parentCustomerNumber)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerNumber = customerNumber;
        ParentCustomerNumber = parentCustomerNumber;
    }

    public static ReducedCustomerFromRepository Create(CustomerId customerId, CustomerName customerName, CustomerNumber customerNumber, CustomerNumber parentCustomerNumber)
    {
        return new ReducedCustomerFromRepository(customerId, customerName, customerNumber, parentCustomerNumber);
    }

    public static ReducedCustomerFromRepository Empty()
    {
        return new ReducedCustomerFromRepository();
    }
}