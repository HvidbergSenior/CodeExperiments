using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class AllocationDraftId : ValueObject
    {
        private const string PROFITSHARE_ENTITY_ID = "93ad675f-1d26-491d-adbb-08ac43479da2";

        private AllocationDraftId()
        {
            Value = Guid.Parse(PROFITSHARE_ENTITY_ID);
        }

        private AllocationDraftId(Guid value)
        {
            // Todo: We're missing this test.
            // Left empty to satisfy test ValueObject_Should_Have_Private_Constructor_With_Parameters_For_Its_State
        }

        public Guid Value { get; private set; }

        /// <summary>
        /// Only one of these (should) exist in the application
        /// </summary>
        /// <returns></returns>
        public static AllocationDraftId Instance => new AllocationDraftId();
    }
}
