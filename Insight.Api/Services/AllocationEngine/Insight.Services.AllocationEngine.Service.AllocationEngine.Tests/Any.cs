using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Quantity = Insight.IncomingDeclarations.Domain.Incoming.Quantity;

namespace Insight.Services.AllocationEngine.Service.AllocationEngine.Tests
{
    internal static class Any
    {
        public static Random random = new Random();

        public static async Task<IEnumerable<CustomerId>> AddCustomers(this ICustomerRepository customerRepository, int count = 1, double minCo2Target = 0.5f, double maxCo2Target = 0.8f)
        {
            var customers = new List<CustomerId>();
            for (int i = 0; i < count; i++)
            {
                var customer = Customers.Tests.Any.Customer();
                customer.SetCO2Target(CO2Target.Create(Convert.ToDecimal(Range(minCo2Target, maxCo2Target))));
                customers.Add(customer.CustomerId);
                await customerRepository.Add(customer);
            }
            await customerRepository.SaveChanges();
            return customers;
        }

        public static async Task AddIncomingDeclarations(this IIncomingDeclarationRepository incomingDeclarationRepository, DateOnly startDate, DateOnly endDate, Product product, Country country, PlaceOfDispatch placeOfDispatch, Quantity quantity, IncomingDeclarationState incomingDeclarationState, int count = 1, double minGhgr = 0.5f, double maxGhgr = 0.98f)
        {
            var daysInBetween = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MaxValue)).Days;

            for (int i = 0; i < count; i++)
            {
                var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate.AddDays(random.Next(0, daysInBetween))),
                                                                                            product,
                                                                                            country,
                                                                                            placeOfDispatch,
                                                                                            GHGEmissionSaving.Create(Convert.ToDecimal(Range(minGhgr,maxGhgr))),
                                                                                            quantity);
                incomingDeclaration.SetIncomingDeclarationState(incomingDeclarationState);
                await incomingDeclarationRepository.Add(incomingDeclaration);
            }
            await incomingDeclarationRepository.SaveChanges();
        }

        public static async Task AddFuelTransactions(this IFuelTransactionsRepository fuelTransactionsRepository,
                                                     FuelTransactionCustomerId customerId,
                                                     CustomerNumber customerNumber,
                                                     CustomerName customerName,
                                                     CustomerSegment customerSegment,
                                                     CustomerType customerType,
                                                     DateOnly startDate,
                                                     DateOnly endDate,
                                                     ProductNumber productNumber,
                                                     ProductName productName,
                                                     CompanyName companyName,
                                                     FuelTransactionCountry country,
                                                     StationName stationName,
                                                     StationNumber stationNumber,
                                                     FuelTransactions.Domain.Quantity quantity,
                                                     Location location,
                                                     int count = 1)
        {
            var daysInBetween = Convert.ToInt32((endDate.ToDateTime(TimeOnly.MaxValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days * 0.8M);
            
            for (int i = 0; i < count; i++)
            {
                var fuelTransaction = FuelTransactions.Tests.Any.FuelTransaction(startDate.AddDays(random.Next(daysInBetween == 0 ? 0 : 1, daysInBetween))
                    .ToDateTime(TimeOnly.MinValue.AddHours(random.Next(2,23)).AddMinutes(random.Next(0,59))), customerId, customerNumber, customerName, customerType, customerSegment, productNumber, productName, country,stationName, stationNumber, quantity, location, companyName);
                if(fuelTransaction.FuelTransactionTimeStamp < startDate.ToDateTime(TimeOnly.MinValue) || fuelTransaction.FuelTransactionTimeStamp> endDate.ToDateTime(TimeOnly.MaxValue))
                {
                    throw new Exception($"Invald fueltransaction creation date {fuelTransaction.FuelTransactionTimeStamp}");
                }
                await fuelTransactionsRepository.Add(fuelTransaction);
            }
        }

        private static Double Range(Double minValue, Double maxValue)
        {
            var x = random.NextDouble();

            return x * maxValue + (1 - x) * minValue;
        }
    }
}
