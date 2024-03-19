using Insight.IncomingDeclarations.Domain.Incoming;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Insight.Services.AllocationEngine.Domain.Converters
{
    public class IncomingDeclarationIdAndQuantityCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IncomingDeclarationIdAndQuantityCollection));
        }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new IncomingDeclarationIdAndQuantityCollection();
            }
            else
            {
                JObject obj = JObject.Load(reader);
                var dict = new IncomingDeclarationIdAndQuantityCollection();
                foreach (var o in obj)
                {
                    var incomingDeclarationId = IncomingDeclarationId.Create(Guid.Parse(o.Key));
                    var quantity = (Quantity)serializer.Deserialize(o.Value!.CreateReader(), typeof(Quantity))!;
                    dict.Add(incomingDeclarationId, quantity);
                }
                return dict;
            }
        }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<IncomingDeclarationId, Insight.IncomingDeclarations.Domain.Incoming.Quantity> foo in (IncomingDeclarationIdAndQuantityCollection)value)
            {
                writer.WritePropertyName(foo.Key.Value.ToString());
                serializer.Serialize(writer, foo.Value);
            }
            writer.WriteEndObject();
        }
    }
}
