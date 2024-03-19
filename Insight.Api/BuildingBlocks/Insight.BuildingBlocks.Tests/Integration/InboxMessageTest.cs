using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Integration.Inbox;

namespace Insight.BuildingBlocks.Tests.Integration
{
    public class InboxMessageTest
    {
#pragma warning disable CA1812 // Avoid uninstantiated internal classes

        internal sealed class UserCreatedEvent : IntegrationEvent
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
        {
            public UserCreatedEvent(Guid id, DateTimeOffset occuredOn) : base(id, occuredOn)
            {
            }
        }

        internal sealed class CreateUserCommand : IInternalCommand, ICommand
        {
            public int Age { get; private set; }
            public string Name { get; private set; } = string.Empty;

            public List<string> StringCollection { get; private set; } = default!;

            public CreateUserCommand(int age, string name, List<string> strings)
            {
                Age = age;
                Name = name;
                StringCollection = strings;
            }

            public CreateUserCommand()
            {
            }
        }

        [Fact]
        public void Will_Create_InboxMessage_With_MessageType()
        {
            var strings = new List<string>
            {
                "A",
                "B",
            };
            var cmd = new CreateUserCommand(42, "mjomeet", strings);
            var inboxMessage = InboxMessage.From(cmd, Guid.NewGuid());

            Assert.Equal(cmd.GetType().AssemblyQualifiedName, inboxMessage.MessageType);
        }

        [Fact]
        public void Will_Serialize_And_Deserialize()
        {
            var strings = new List<string>
            {
                "A",
                "B",
            };
            var cmd = new CreateUserCommand(42, "mjomeet", strings);
            var serializer = new InboxMessageSerializer();
            var json = serializer.Serialize(cmd, typeof(UserCreatedEvent));
            var type = cmd.GetType().AssemblyQualifiedName;
            var obj = (CreateUserCommand)serializer.Deserialize(json, type!)!;
        
            Assert.NotNull(json);
            //The InboxResolver in InboxMessageSerializer generates backing field properties.
            //Because of that the output of the json will be times 2
            Assert.Equal(2 * strings.Count, obj.StringCollection.Count);
        }
    }
}