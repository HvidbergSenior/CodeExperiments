using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.Customers.Domain;
using Insight.Customers.Infrastructure.Repositories;
using Marten;
using Microsoft.IdentityModel.Tokens;

namespace Insight.Tests.End2End.TestData
{
    public static class CustomerTestData
    {
        public static async Task<IEnumerable<Customer>> SeedWithCustomer(int count, 
            WebAppFixture fixture, bool useExistingDeclarationsIfAny = true, string? customerNumber = null, string? customerBillToNumber = null)
        {
            Fixture autoFixture = new();

            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher =
                (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;
            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);

            var customerRepository = new CustomerRepository(documentSession, entityEventsPublisher);

            var customers = new List<Customer>();

            var hasCustomers = await customerRepository.AnyAsync(CancellationToken.None);

            if (!useExistingDeclarationsIfAny || !hasCustomers)
            {
                for (var i = 0; i < count; i++)
                {
                   autoFixture.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-5), maxDate: DateTime.Now));
                    var date = autoFixture.Create<DateTime>();
                    var customerNumberDomain = CustomerNumber.Create(autoFixture.Create<string>());
                    var customerBillToNumberDomain = CustomerBillToNumber.Create(autoFixture.Create<string>());
                    if (customerNumber != null)
                    {
                        customerNumberDomain = CustomerNumber.Create(customerNumber);
                    }
                    if (customerBillToNumber != null)
                    {
                        customerBillToNumberDomain = CustomerBillToNumber.Create(customerBillToNumber);
                    }
                    customers.Add(Customer.Create(
                        CompanyId.Create(autoFixture.Create<Guid>()),
                        CustomerId.Create(autoFixture.Create<Guid>()), 
                        CustomerDetails.Create(customerNumberDomain,
                            CustomerAddress.Create(autoFixture.Create<string>()), 
                            CustomerBillToName.Create(autoFixture.Create<string>()), 
                            customerBillToNumberDomain,
                            CustomerCity.Create(autoFixture.Create<string>()), 
                            CustomerDeliveryType.Create(autoFixture.Create<string>()),
                            CustomerIndustry.Create(autoFixture.Create<string>()),
                            CustomerName.Create(autoFixture.Create<string>()),
                            CustomerPostCode.Create(autoFixture.Create<string>()),
                            CustomerCountryRegion.Create(autoFixture.Create<string>())),
                        BalanceLcy.Create(autoFixture.Create<decimal>()),
                        BalanceDueLcy.Create(autoFixture.Create<decimal>()),
                        OutstandingOrdersLcy.Create(autoFixture.Create<decimal>()), 
                        NumberNumber.Create(autoFixture.Create<string>()), 
                        PdiAndLdPointNumber.Create(autoFixture.Create<string>()), 
                        VatRegNumber.Create(autoFixture.Create<string>()), 
                        OrganisationNumber.Create(autoFixture.Create<string>()), 
                        PaymentTermsCode.Create(autoFixture.Create<string>()), 
                        ShipmentMethodCode.Create(autoFixture.Create<string>()), 
                        ShippingAgentCode.Create(autoFixture.Create<string>()), 
                        SalesPerson.Create(autoFixture.Create<string>()), 
                        SourceETag.Create(autoFixture.Create<string>()), 
                        CreditLimit.Create(autoFixture.Create<decimal>()), 
                        Blocked.Create(autoFixture.Create<string>()), 
                        CardCustomer.Create(autoFixture.Create<bool>())
                    ));
                }

                await customerRepository.Add(customers);
                await customerRepository.SaveChanges();
                await uow.Commit(CancellationToken.None);
            }
            else
            {
                customers = (await customerRepository.GetAllByPaging(1, count, CancellationToken.None)).ToList();
            }
            
            return customers;
        }

        public static async Task DeleteAllCustomers(WebAppFixture fixture)
        {
            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher =
                (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;
            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);

            var customerRepository = new CustomerRepository(documentSession, entityEventsPublisher);

            var allCustomers = await customerRepository.GetAllByPaging(1, 10000);
            await customerRepository.Delete(allCustomers, CancellationToken.None);
            await customerRepository.SaveChanges();
            await uow.Commit(CancellationToken.None);
        }
    }
}