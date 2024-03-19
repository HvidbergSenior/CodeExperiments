using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.Auth
{
    public sealed class RefreshTokenId : ValueObject
    {
        public Guid Value { get; private set; }

        private RefreshTokenId()
        {

        }

        private RefreshTokenId(Guid value)
        {
            Value = value;
        }

        public static RefreshTokenId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(value));
            }

            return new RefreshTokenId(value);
        }

        public static RefreshTokenId Empty()
        {
            return new RefreshTokenId(Guid.Empty);
        }
    }
}
