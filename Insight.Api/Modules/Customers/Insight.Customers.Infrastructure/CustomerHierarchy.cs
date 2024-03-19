using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure.Environment;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using JasperFx.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Insight.Customers.Infrastructure;

public class CustomerHierarchy : ICustomerHierarchy
{
    private const string MEMORY_CACHE_PREFIX = "CustomerHierarchy_";
    private const string MEMORY_CACHE_KEYNAME = "CustomerNodes";
    private const int CACHE_ENTRY_LIFE_TIME_IN_HOURS = 1;
    private readonly ICustomerRepository customerRepository;
    private readonly IMemoryCache memoryCache;

    private List<ReducedCustomerFromRepository> reducedCustomersFromRepository =
        new List<ReducedCustomerFromRepository>();
    private List<CustomerNode> customerNodes =
        new List<CustomerNode>();

    private readonly IEnvironment environment;

    public CustomerHierarchy(ICustomerRepository customerRepository, IMemoryCache memoryCache, IEnvironment environment)
    {
        this.customerRepository = customerRepository;
        this.memoryCache = memoryCache;
        this.environment = environment;
    }

    private async Task UpdateHierarchyAsync(CancellationToken cancellationToken)
    {
        await UpdateCustomersFromRepository(cancellationToken);
        UpdateCustomerNodes();
    }

    public async Task<IReadOnlyList<CustomerNode>> GetCustomerNodes(CancellationToken cancellationToken)
    {
        List<CustomerNode>? customerNodesReturn;
        if (environment.IsTestEnvironment())
        {
            //Do not use the cache on test environment as new customers are not seen inmediately as it requieres the customer updated event to be executed
            await UpdateHierarchyAsync(cancellationToken);
            customerNodesReturn = customerNodes;
        }
        else
        {
            customerNodesReturn = await memoryCache.GetOrCreateAsync(MEMORY_CACHE_PREFIX + MEMORY_CACHE_KEYNAME,
                async entry =>
                {
                    entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(CACHE_ENTRY_LIFE_TIME_IN_HOURS);
                    await UpdateHierarchyAsync(cancellationToken);
                    return customerNodes;
                });
        }
        return customerNodesReturn != null
            ? customerNodesReturn.AsReadOnly()
            : (new List<CustomerNode>()).AsReadOnly();
    }

    private static CustomerNode? FindInListOfCustomerNodes(IReadOnlyList<CustomerNode> customerNodes,
        CustomerNumber customerNumber)
    {
        //TODO: Test this
        foreach (var customer in customerNodes)
        {
            if (customer.CustomerNumber.Value == customerNumber.Value)
            {
                return customer;
            }
            else if (customer.Children.Any())
            {
                var possibleMatch = FindInListOfCustomerNodes(customer.Children, customerNumber);
                if (possibleMatch != null)
                {
                    return possibleMatch;
                }
            }
        }

        return null;
    }

    private void UpdateCustomerNodes()
    {
        var newCustomerNodes = new List<CustomerNode>();
        var missingCustomers = new List<ReducedCustomerFromRepository>();
        var missingCustomersNew = new List<ReducedCustomerFromRepository>();
        missingCustomers.AddRange(reducedCustomersFromRepository);
        var oldMissingCustomersCount = missingCustomers.Count;
        do
        {
            oldMissingCustomersCount = missingCustomers.Count();
            missingCustomersNew = new List<ReducedCustomerFromRepository>();
            foreach (var customer in missingCustomers)
            {
                if (customer.ParentCustomerNumber.Value.IsEmpty() || customer.ParentCustomerNumber.Value == customer.CustomerNumber.Value)
                {
                    //Root node, add it right away
                    var customerNode = CustomerNode.Create(customer.CustomerId, customer.CustomerName,
                        customer.CustomerNumber, null, new List<CustomerNode>());
                    newCustomerNodes.Add(customerNode);
                    //missingCustomers.Remove(customer);
                }
                else
                {
                    //Need to add it as children of some if found
                    /*var nodeInList = newCustomerNodes
                        .FirstOrDefault(c => customer.ParentCustomerNumber.Value == c.CustomerNumber.Value);*/
                    //TODO: What about looking in children and children's children etc.
                    var nodeInList = FindInListOfCustomerNodes(newCustomerNodes, customer.ParentCustomerNumber);

                    if (nodeInList != null)
                    {
                        //We found the node in the list so we can take this one out also
                        var customerNode = CustomerNode.Create(customer.CustomerId, customer.CustomerName,
                            customer.CustomerNumber, nodeInList.CustomerId, new List<CustomerNode>());
                        nodeInList.AddChildren(customerNode);
                        //missingCustomers.Remove(customer);
                    }
                    else
                    {
                        //We didn't find the parent (yet). Maybe next iteration will find it
                        missingCustomersNew.Add(customer);
                    }
                }
            }

            missingCustomers = missingCustomersNew;
        } while (missingCustomers.Count() != oldMissingCustomersCount);

        if (missingCustomers.Any())
        {
            //Oh no, customers with a parent not found, let's just add them all anyway as root then
            foreach (var customer in missingCustomers)
            {
                var customerNode = CustomerNode.Create(customer.CustomerId, customer.CustomerName,
                    customer.CustomerNumber, null, new List<CustomerNode>());
                newCustomerNodes.Add(customerNode);
            }
        }

        customerNodes = newCustomerNodes;
    }

    private async Task UpdateCustomersFromRepository(CancellationToken cancellationToken)
    {
        int pageStart = 1;
        const int pageSize = 10;
        IEnumerable<ReducedCustomerFromRepository> customersFetched;
        do
        {
            customersFetched = (await customerRepository.GetAllByPaging(pageStart, pageSize, cancellationToken))
                .ToList().Select(c =>
                    ReducedCustomerFromRepository.Create(
                        CustomerId.Create(c.CustomerId.Value),
                        CustomerName.Create(c.CustomerDetails.CustomerName.Value),
                        CustomerNumber.Create(c.CustomerDetails.CustomerNumber.Value),
                        CustomerNumber.Create(c.CustomerDetails.CustomerBillToNumber.Value)));
            reducedCustomersFromRepository.AddRange(customersFetched);
            pageStart += 1;
        } while (customersFetched.Any());
    }

    public void ClearCache()
    {
        memoryCache.Remove(MEMORY_CACHE_PREFIX + MEMORY_CACHE_KEYNAME);
    }
}