using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class DeclarationRowNumber : ValueObject
    {
        public int Value { get; private set; }

        private DeclarationRowNumber()
        {
            Value = int.MinValue;
        }

        private DeclarationRowNumber(int value)
        {
            Value = value;
        }

        public static DeclarationRowNumber Create(int value)
        {
            return new DeclarationRowNumber(value);
        }

        public static DeclarationRowNumber Empty()
        {
            return new DeclarationRowNumber();
        }
    }
}
