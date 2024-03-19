using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Parsing
{
    public sealed class ColumnParsingError : ValueObject
    {
        private ColumnParsingError(int row, int column, string posNumber, string errorMessage)
        {
            Row = row;
            Column = column;
            PosNumber = posNumber;
            ErrorMessage = errorMessage;
            PosNumber = posNumber;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
        public string PosNumber { get; private set; }
        public string ErrorMessage { get; private set; }

        private ColumnParsingError()
        {
            Row = 0;
            Column = 0;
            ErrorMessage = string.Empty;
            PosNumber = string.Empty;
        }

        public static ColumnParsingError Create(int row, int column, string posNumber, string errorMessage)
        {
            return new ColumnParsingError(row, column, posNumber, errorMessage);
        }

        public static ColumnParsingError Empty()
        {
            return new ColumnParsingError(0, 0, string.Empty, string.Empty);
        }
    }
}
