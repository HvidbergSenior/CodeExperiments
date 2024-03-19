using AutoFixture;
using FluentAssertions;
using Insight.FuelTransactions.Domain;
using Xunit;

namespace Insight.Services.BusinessCentralConnector.Test.FuelTransactionsTests
{
    public class FuelTransactionUniquenessTests
    {
        private readonly Fixture autoFixture;
        public FuelTransactionUniquenessTests()
        {
            autoFixture = new Fixture();
        }

        [Fact]
        public void FuelTransactionsMustBeUnique()
        {
            var fuelTransactions = new List<FuelTransaction>();
            for(var i = 0; i < 100000;i++)
            {
                fuelTransactions.Add(Any.FuelTransaction());
            }

            var baddies = fuelTransactions.GroupBy(c => c.ItemHash).Where(c => c.Count() > 1);

            //var serialized = new List<string>();
            //
            //foreach(var baddie in baddies)
            //{
            //    foreach(var t in baddie)
            //    {
            //        serialized.Add(JsonConvert.SerializeObject(t));
            //    }
            //}

            baddies.Should().BeEmpty();
        }
    }
}
