using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Xunit;
using AllocationId = Insight.Services.AllocationEngine.Domain.AllocationId;
using Quantity = Insight.IncomingDeclarations.Domain.Incoming.Quantity;

namespace Insight.Services.AllocationEngine.Service.AllocationEngine.Tests
{
    public class DraftAllocationTests
    {
        [Fact]
        public void ShouldBeAbleToAddAllocationToDraft()
        {
            var sut = AllocationDraft.Create();

            var incomingDeclarationId = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations.Add(incomingDeclarationId, Quantity.Create(10));
            var fuelTransactionIds = Enumerable.Range(1, 10).Select(x => FuelTransactionId.Create(Guid.NewGuid())).ToArray();

            var customerDetails = CustomerDetails.Create(
                CustomerNumber.Create("5345345345"),
                CustomerAddress.Create("Address1"),
                CustomerBillToName.Create("Cbn1"),
                CustomerBillToNumber.Create("Cb1"),
                CustomerCity.Create("City1"),
                CustomerDeliveryType.Create("Cdtype1"),
                CustomerIndustry.Create("Industry1"),
                CustomerName.Create("Customer 1"),
                CustomerPostCode.Create("CPC1"),
                CustomerCountryRegion.Create("Country1"));

            var fuelTransactionCountry = FuelTransactionCountry.Create("Denmark");
            var productName = ProductName.Create("GreatProduct");

            var allocation = Allocation.Create(customerDetails,fuelTransactionCountry, productName, DateOnly.MinValue, DateOnly.MaxValue);
            allocation.SetTransactionIds(fuelTransactionIds);
            allocation.SetAllocations(incomingDeclarations);

            sut.AddAllocation(allocation);
            
            sut.Allocations.Should().HaveCount(1);
            sut.FuelTransactionIds.Should().HaveCount(10);
        }

        [Fact]
        public void ShouldBeAbleToRemoveAllocationFromDraft()
        {
            var sut = AllocationDraft.Create();

            var incomingDeclarationId = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations.Add(incomingDeclarationId, Quantity.Create(10));
            var fuelTransactionIds = Enumerable.Range(1, 10).Select(x => FuelTransactionId.Create(Guid.NewGuid())).ToArray();

            var fuelTransactionCountry = FuelTransactionCountry.Create("Denmark");
            var productName = ProductName.Create("GreatProduct");

            var customerDetails = CustomerDetails.Create(
                CustomerNumber.Create("5345345345"),
                CustomerAddress.Create("Address1"),
                CustomerBillToName.Create("Cbn1"),
                CustomerBillToNumber.Create("Cb1"),
                CustomerCity.Create("City1"),
                CustomerDeliveryType.Create("Cdtype1"),
                CustomerIndustry.Create("Industry1"),
                CustomerName.Create("Customer 1"),
                CustomerPostCode.Create("CPC1"),
                CustomerCountryRegion.Create("Country1"));

            var allocation = Allocation.Create(customerDetails, fuelTransactionCountry, productName, DateOnly.MinValue, DateOnly.MaxValue);
            allocation.SetTransactionIds(fuelTransactionIds);
            allocation.SetAllocations(incomingDeclarations);

            sut.AddAllocation(allocation);

            sut.Allocations.Should().HaveCount(1);
            sut.FuelTransactionIds.Should().HaveCount(10);

            sut.RemoveAllocation(allocation.AllocationId);
            sut.Allocations.Should().BeEmpty();
            sut.FuelTransactionIds.Should().BeEmpty();
        }

        [Fact]
        public void ReAddingAllocationToDraftShouldFail()
        {
            var sut = AllocationDraft.Create();

            var incomingDeclarationId = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations.Add(incomingDeclarationId, Quantity.Create(10));
            var fuelTransactionIds = Enumerable.Range(1, 10).Select(x => FuelTransactionId.Create(Guid.NewGuid())).ToArray();

            var customerDetails = CustomerDetails.Create(
                CustomerNumber.Create("5345345345"),
                CustomerAddress.Create("Address1"),
                CustomerBillToName.Create("Cbn1"),
                CustomerBillToNumber.Create("Cb1"),
                CustomerCity.Create("City1"),
                CustomerDeliveryType.Create("Cdtype1"),
                CustomerIndustry.Create("Industry1"),
                CustomerName.Create("Customer 1"),
                CustomerPostCode.Create("CPC1"),
                CustomerCountryRegion.Create("Country1"));
            var fuelTransactionCountry = FuelTransactionCountry.Create("Denmark");
            var productName = ProductName.Create("GreatProduct");

            var allocation = Allocation.Create(customerDetails, fuelTransactionCountry, productName, DateOnly.MinValue, DateOnly.MaxValue);
            allocation.SetTransactionIds(fuelTransactionIds);
            allocation.SetAllocations(incomingDeclarations);
            var firstResult = sut.AddAllocation(allocation);
            
            var secondResult = sut.AddAllocation(allocation);

            sut.Allocations.Should().HaveCount(1);
            sut.FuelTransactionIds.Should().HaveCount(10);
            firstResult.Should().BeTrue();
            secondResult.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotBeAbleToRemoveNonExistingAllocationFromDraft()
        {
            var sut = AllocationDraft.Create();

            var incomingDeclarationId = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations.Add(incomingDeclarationId, Quantity.Create(10));
            var fuelTransactionIds = Enumerable.Range(1, 10).Select(x => FuelTransactionId.Create(Guid.NewGuid())).ToArray();

            var customerDetails = CustomerDetails.Create(
                CustomerNumber.Create("5345345345"),
                CustomerAddress.Create("Address1"),
                CustomerBillToName.Create("Cbn1"),
                CustomerBillToNumber.Create("Cb1"),
                CustomerCity.Create("City1"),
                CustomerDeliveryType.Create("Cdtype1"),
                CustomerIndustry.Create("Industry1"),
                CustomerName.Create("Customer 1"),
                CustomerPostCode.Create("CPC1"),
                CustomerCountryRegion.Create("Country1"));
            var fuelTransactionCountry = FuelTransactionCountry.Create("Denmark");
            var productName = ProductName.Create("GreatProduct");

            var allocation = Allocation.Create(customerDetails, fuelTransactionCountry, productName, DateOnly.MinValue, DateOnly.MaxValue);
            allocation.SetTransactionIds(fuelTransactionIds);
            allocation.SetAllocations(incomingDeclarations);

            sut.AddAllocation(allocation);

            var removeResult = sut.RemoveAllocation(AllocationId.Create(Guid.NewGuid()));
            removeResult.Should().BeFalse();
            sut.Allocations.Should().HaveCount(1);
            sut.FuelTransactionIds.Should().HaveCount(10);
        }

        [Fact]
        public void ShouldNotBeAbleToAddAllocationOnLockedAllocationDrafts()
        {
            var sut = AllocationDraft.Create();
            sut.Lock();

            var incomingDeclarationId = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations.Add(incomingDeclarationId, Quantity.Create(10));
            var fuelTransactionIds = Enumerable.Range(1, 10).Select(x => FuelTransactionId.Create(Guid.NewGuid())).ToArray();

            var customerDetails = CustomerDetails.Create(
                CustomerNumber.Create("5345345345"),
                CustomerAddress.Create("Address1"),
                CustomerBillToName.Create("Cbn1"),
                CustomerBillToNumber.Create("Cb1"),
                CustomerCity.Create("City1"),
                CustomerDeliveryType.Create("Cdtype1"),
                CustomerIndustry.Create("Industry1"),
                CustomerName.Create("Customer 1"),
                CustomerPostCode.Create("CPC1"),
                CustomerCountryRegion.Create("Country1"));
            var fuelTransactionCountry = FuelTransactionCountry.Create("Denmark");
            var productName = ProductName.Create("GreatProduct");

            var allocation = Allocation.Create(customerDetails, fuelTransactionCountry, productName, DateOnly.MinValue, DateOnly.MaxValue);
            allocation.SetTransactionIds(fuelTransactionIds);
            allocation.SetAllocations(incomingDeclarations);

            var result = sut.AddAllocation(allocation);

            result.Should().BeFalse();
            sut.Allocations.Should().BeEmpty();
            sut.FuelTransactionIds.Should().BeEmpty();
        }

        [Fact]
        public void ShouldNotBeAbleToRemoveAllocationOnLockedAllocationDrafts()
        {
            var sut = AllocationDraft.Create();
            var incomingDeclarationId = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations.Add(incomingDeclarationId, Quantity.Create(10));
            var fuelTransactionIds = Enumerable.Range(1, 10).Select(x => FuelTransactionId.Create(Guid.NewGuid())).ToArray();

            var customerDetails = CustomerDetails.Create(
                CustomerNumber.Create("5345345345"),
                CustomerAddress.Create("Address1"),
                CustomerBillToName.Create("Cbn1"),
                CustomerBillToNumber.Create("Cb1"),
                CustomerCity.Create("City1"),
                CustomerDeliveryType.Create("Cdtype1"),
                CustomerIndustry.Create("Industry1"),
                CustomerName.Create("Customer 1"),
                CustomerPostCode.Create("CPC1"),
                CustomerCountryRegion.Create("Country1"));
            var fuelTransactionCountry = FuelTransactionCountry.Create("Denmark");
            var productName = ProductName.Create("GreatProduct");

            var allocation = Allocation.Create(customerDetails, fuelTransactionCountry, productName,  DateOnly.MinValue, DateOnly.MaxValue);
            allocation.SetTransactionIds(fuelTransactionIds);
            allocation.SetAllocations(incomingDeclarations);

            sut.AddAllocation(allocation);
            
            sut.Lock();

            var result = sut.RemoveAllocation(allocation.AllocationId);

            result.Should().BeFalse();
            sut.Allocations.Should().HaveCount(1);
            sut.FuelTransactionIds.Should().HaveCount(10);
        }

        [Fact]
        public void ShouldNotBeAbleToAllocateAlreadyAllocatedFuelTransactions()
        {
            var sut = AllocationDraft.Create();
            var incomingDeclarationId = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations.Add(incomingDeclarationId, Quantity.Create(10));
            var fuelTransactionIds = Enumerable.Range(1, 10).Select(x => FuelTransactionId.Create(Guid.NewGuid())).ToArray();

            var customerDetails = CustomerDetails.Create(
                CustomerNumber.Create("5345345345"),
                CustomerAddress.Create("Address1"),
                CustomerBillToName.Create("Cbn1"),
                CustomerBillToNumber.Create("Cb1"),
                CustomerCity.Create("City1"),
                CustomerDeliveryType.Create("Cdtype1"),
                CustomerIndustry.Create("Industry1"),
                CustomerName.Create("Customer 1"),
                CustomerPostCode.Create("CPC1"),
                CustomerCountryRegion.Create("Country1"));
            var fuelTransactionCountry = FuelTransactionCountry.Create("Denmark");
            var productName = ProductName.Create("GreatProduct");

            var allocation = Allocation.Create(customerDetails, fuelTransactionCountry, productName, DateOnly.MinValue, DateOnly.MaxValue);
            allocation.SetTransactionIds(fuelTransactionIds);
            allocation.SetAllocations(incomingDeclarations);

            sut.AddAllocation(allocation);

            var incomingDeclarationId2 = IncomingDeclarationId.Create(Guid.NewGuid());

            var incomingDeclarations2 = new IncomingDeclarationIdAndQuantityCollection();
            incomingDeclarations2.Add(incomingDeclarationId2, Quantity.Create(10));

            var allocation2 = Allocation.Create(customerDetails, fuelTransactionCountry, productName, DateOnly.MinValue, DateOnly.MaxValue);
            allocation2.SetTransactionIds(fuelTransactionIds);
            allocation2.SetAllocations(incomingDeclarations2);

            var result = sut.AddAllocation(allocation2);

            result.Should().BeFalse();
            sut.Allocations.Should().HaveCount(1);
            sut.FuelTransactionIds.Should().HaveCount(10);
        }

        [Fact]
        public void ShouldNotBeAbleToClearDraftIfLocked()
        {
            var sut = AllocationDraft.Create();
            sut.Lock();

            sut.AllowClear.Should().BeFalse();
        }
    }
}
