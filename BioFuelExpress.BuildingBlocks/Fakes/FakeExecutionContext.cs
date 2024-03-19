using BioFuelExpress.BuildingBlocks.Application;

namespace BioFuelExpress.BuildingBlocks.Fakes
{
    public class FakeExecutionContext : IExecutionContext
    {
        public Guid UserId { get; }

        public string UserName { get; }
        public string Email { get; private set; }

        public FakeExecutionContext() : this(Guid.NewGuid())
        {
        }

        public void SetNewEmail(string email)
        {
            Email = email;
        }

        public FakeExecutionContext(Guid userId, string userName = "test", string email = "me@me.dk")
        {
            UserId = userId;
            UserName = userName;
            Email = email;
        }
    }
}