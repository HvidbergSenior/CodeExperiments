using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public class CustomerHierarchyHelper
{
    public static CustomerNode? FindInListOfCustomerNodes(IReadOnlyList<CustomerNode> customerNodes,
        CustomerId customerId)
    {
        //TODO: Test this
        foreach (var customer in customerNodes)
        {
            if (customer.CustomerId.Value == customerId.Value)
            {
                return customer;
            }
            else if (customer.Children.Any())
            {
                var possibleMatch = FindInListOfCustomerNodes(customer.Children, customerId);
                if (possibleMatch != null)
                {
                    return possibleMatch;
                }
            }
        }

        return null;
    }
}