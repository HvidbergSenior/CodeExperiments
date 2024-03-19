using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class PlaceOfDispatch : ValueObject
    {
        public string Value { get; private set; }

        private PlaceOfDispatch()
        {
            Value = string.Empty;
        }

        private PlaceOfDispatch(string value)
        {
            Value = value;
        }

        public static PlaceOfDispatch Create(string value)
        {
            return new PlaceOfDispatch(value);
        }

        public static PlaceOfDispatch Empty()
        {
            return new PlaceOfDispatch();
        }
    }
}
