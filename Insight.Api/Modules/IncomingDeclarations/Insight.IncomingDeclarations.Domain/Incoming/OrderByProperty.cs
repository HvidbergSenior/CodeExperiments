using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class OrderByProperty : ValueObject
    {
        public string Value { get; private set; }

        private OrderByProperty()
        {
            Value = string.Empty;
        }

        private OrderByProperty(string value)
        {
            Value = value;
        }

        public static OrderByProperty Create(string value)
        {
            return new OrderByProperty(value);
        }

        public static OrderByProperty Empty()
        {
            return new OrderByProperty();
        }
    }
}
