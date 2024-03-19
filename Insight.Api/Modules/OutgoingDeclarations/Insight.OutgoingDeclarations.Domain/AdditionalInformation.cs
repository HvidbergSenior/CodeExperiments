using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class AdditionalInformation : ValueObject
    {
        public string Value { get; private set; }

        private AdditionalInformation()
        {
            Value = string.Empty;;
        }

        private AdditionalInformation(string value)
        {
            Value = value;
        }

        public static AdditionalInformation Create(string value)
        {
            return new AdditionalInformation(value);
        }

        public static AdditionalInformation Empty()
        {
            return new AdditionalInformation();
        }
    }
}