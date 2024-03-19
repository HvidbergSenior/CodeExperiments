using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Tests.Domain
{
    public class DomainEventTests
    {
        internal sealed class TestDomainEvent : DomainEvent
        {
            public TestDomainEvent()
            {
            }
        }

        [Fact]
        public void Will_Have_Id_When_Created()
        {
            var evt = new TestDomainEvent();
            Assert.IsType<Guid>(evt.Id);
        }
    }
}
