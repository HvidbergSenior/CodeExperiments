using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Serialization;
using Newtonsoft.Json;

namespace Insight.BuildingBlocks.Integration.Inbox
{
    public class InboxMessageSerializer
    {
        private readonly JsonSerializerSettings settings;

        public InboxMessageSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new InboxResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        public string Serialize(IInternalCommand command, Type? type)
        {
            return JsonConvert.SerializeObject(command, type, settings);
        }

        public object? Deserialize(string payload, string messageType)
        {
            Type? type = Type.GetType(messageType);
            return JsonConvert.DeserializeObject(payload, type, settings);
        }
    }
}