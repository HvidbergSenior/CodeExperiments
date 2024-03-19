using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class FulfillsMeasuresForLowILUCRiskFeedstocks : ValueObject
    {
        public bool Value { get; private set; }

        private FulfillsMeasuresForLowILUCRiskFeedstocks()
        {
            Value = false;
        }

        private FulfillsMeasuresForLowILUCRiskFeedstocks(bool value)
        {
            Value = value;
        }

        public static FulfillsMeasuresForLowILUCRiskFeedstocks Create(bool value)
        {
            return new FulfillsMeasuresForLowILUCRiskFeedstocks(value);
        }

        public static FulfillsMeasuresForLowILUCRiskFeedstocks None()
        {
            return new FulfillsMeasuresForLowILUCRiskFeedstocks();
        }
    }
}
