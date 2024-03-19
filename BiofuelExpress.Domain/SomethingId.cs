using BioFuelExpress.BuildingBlocks.Domain;

namespace BioFuelExpress.Domain
{
    public sealed class SomethingId : ValueObject
    {
        public Guid Value { get; private set; }

        private SomethingId()
        {
            Value = Guid.Empty;
        }

        private SomethingId(Guid value)
        {
            Value = value;
        }

        public static SomethingId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new SomethingId(id);
        }

        public static SomethingId Empty()
        {
            return new SomethingId(Guid.Empty);
        }
    }
}
