using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Insight.Services.AllocationEngine.Domain.Converters
{
    public class AllocationCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(AllocationCollection));
        }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new AllocationCollection();
            }
            else
            {
                JObject obj = JObject.Load(reader);
                var dict = new AllocationCollection();
                foreach (var o in obj)
                {
                    var allocationId = AllocationId.Create(Guid.Parse(o.Key));
                    //var allocation = o.Value.ToObject<Allocation>();
                    var allocation = (Allocation)serializer.Deserialize(o.Value!.CreateReader(), typeof(Allocation))!;
                    dict.Add(allocationId, allocation);
                }
                return dict;
            }
        }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<AllocationId, Allocation> foo in (AllocationCollection)value)
            {
                writer.WritePropertyName(foo.Key.Value.ToString());                
                serializer.Serialize(writer, foo.Value);
            }
            writer.WriteEndObject();
        }
    }
}
