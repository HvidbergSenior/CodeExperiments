using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Application.GetOutgoingFuelTransactions;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.OutgoingFuelTransactions;
using Insight.FuelTransactions.Domain.Stock;
using Moq;
using PaginationParameters = Insight.FuelTransactions.Domain.PaginationParameters;
using SortingParameters = Insight.FuelTransactions.Domain.SortingParameters;

namespace Insight.FuelTransactions.Tests.Application.GetOutgoingFuelTransactions
{
    public class GetOutgoingFuelTransactionTests
    {
        [Fact]
        public async Task MakeSureHandlerWorks()
        {
            var repo = new Mock<IOutgoingFuelTransactionsRepository>();            
            var outgoingFuelTransactions = new List<OutgoingFuelTransaction>()
            {
                Any.OutgoingFuelTransaction()
            };

            repo.Setup(c => c.GetByAggregatedFuelTransactionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<SortingParameters>(), It.IsAny<FilteringParameters>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((outgoingFuelTransactions, outgoingFuelTransactions.Count, outgoingFuelTransactions.Sum(c=> c.Quantity.Value)));

            var sut = new GetOutgoingFuelTransactionsQueryHandler(repo.Object);

            var paginationParameters = PaginationParameters.Create(0, 10);
            var sortingParameters = SortingParameters.Create(true, "CustomerNumber");
            var filteringParameters = FilteringParameters.Create(DatePeriod.Always(), ProductName.Empty(), BuildingBlocks.Domain.CustomerName.Empty(), CompanyName.Empty());
            var query = GetOutgoingFuelTransactionsQuery.Create(paginationParameters, sortingParameters, filteringParameters);

            var response = await sut.Handle(query, CancellationToken.None);
            response.Should().NotBeNull();
            response.OutgoingFuelTransactions.Should().HaveCount(1);
        }

        [Fact]
        public async Task MakeSureHandlerWorks2()
        {
            var repo = new Mock<IOutgoingFuelTransactionsRepository>();            
            var outgoingFuelTransactions = Enumerable.Range(1, 100).Select(c => Any.OutgoingFuelTransaction()).ToList();

            repo.Setup(c => c.GetByAggregatedFuelTransactionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<SortingParameters>(), It.IsAny<FilteringParameters>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((int x, int y, SortingParameters z, FilteringParameters a, CancellationToken b) => (outgoingFuelTransactions.Take(y).ToList(), outgoingFuelTransactions.Count, outgoingFuelTransactions.Sum(c=> c.Quantity.Value)));

            var sut = new GetOutgoingFuelTransactionsQueryHandler(repo.Object);

            var paginationParameters = PaginationParameters.Create(0, 10);
            var sortingParameters = SortingParameters.Create(true, "CustomerNumber");
            var filteringParameters = FilteringParameters.Create(DatePeriod.Always(), ProductName.Empty(), BuildingBlocks.Domain.CustomerName.Empty(), CompanyName.Empty());
            var query = GetOutgoingFuelTransactionsQuery.Create(paginationParameters, sortingParameters, filteringParameters);

            var response = await sut.Handle(query, CancellationToken.None);
            response.Should().NotBeNull();
            response.OutgoingFuelTransactions.Should().HaveCount(10);
            response.HasMoreOutgoingFuelTransactions.Should().BeTrue();
            response.TotalAmountOfOutgoingFuelTransactions.Should().Be(outgoingFuelTransactions.Count);
        }
    }
}
