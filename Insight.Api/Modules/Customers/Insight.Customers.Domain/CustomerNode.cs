using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class CustomerNode : ValueObject
{
    public CustomerId CustomerId { get; private set; }
    public CustomerName CustomerName { get; private set; }
    public CustomerNumber CustomerNumber { get; private set; }
    public CustomerId? Parent { get; private set; }
    public List<CustomerNode> Children { get; private set; }

    private CustomerNode()
    {
        CustomerId = CustomerId.Empty();
        CustomerName = CustomerName.Empty();
        CustomerNumber = CustomerNumber.Empty();
        Children = new List<CustomerNode>();
    }

    private CustomerNode(CustomerId customerId, CustomerName customerName, CustomerNumber customerNumber, CustomerId? parent, List<CustomerNode> children)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerNumber = customerNumber;
        Parent = parent;
        Children = children;
    }

    public static CustomerNode Create(CustomerId customerId, CustomerName customerName, CustomerNumber customerNumber, CustomerId? parent, List<CustomerNode> children)
    {
        return new CustomerNode(customerId, customerName, customerNumber, parent, children);
    }

    public static CustomerNode Empty()
    {
        return new CustomerNode();
    }

    public void AddChildren(CustomerNode customerNode)
    {
        Children.Add(customerNode);
    }
}