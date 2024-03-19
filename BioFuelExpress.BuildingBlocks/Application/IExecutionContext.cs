namespace BioFuelExpress.BuildingBlocks.Application
{
    public interface IExecutionContext
    {
        public Guid UserId { get; }
        public string UserName { get; }
        public string Email { get; }
    }
}
