using Newtonsoft.Json;

namespace Insight.BuildingBlocks.Serialization
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        private readonly JsonSerializerSettings settings;

        public JsonSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver(),
            };
        }

        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public T? Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<T>(json, settings);

            return result ?? default;
        }
    }
}