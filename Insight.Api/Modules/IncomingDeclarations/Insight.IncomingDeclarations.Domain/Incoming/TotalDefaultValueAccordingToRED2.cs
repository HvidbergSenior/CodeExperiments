using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class TotalDefaultValueAccordingToRED2 : ValueObject
    {
        public bool Value { get; private set; }

        private TotalDefaultValueAccordingToRED2()
        {
            Value = false;
        }

        private TotalDefaultValueAccordingToRED2(bool value)
        {
            Value = value;
        }

        public static TotalDefaultValueAccordingToRED2 Create(bool value)
        {
            return new TotalDefaultValueAccordingToRED2(value);
        }

        public static TotalDefaultValueAccordingToRED2 None()
        {
            return new TotalDefaultValueAccordingToRED2();
        }
    }
}
