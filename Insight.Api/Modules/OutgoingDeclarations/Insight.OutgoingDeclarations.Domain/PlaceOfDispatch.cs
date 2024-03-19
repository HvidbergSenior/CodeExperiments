using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
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

        public static PlaceOfDispatch Create(string company)
        {
            return new PlaceOfDispatch(company);
        }

        public static PlaceOfDispatch Empty()
        {
            return new PlaceOfDispatch();
        }
    }
}