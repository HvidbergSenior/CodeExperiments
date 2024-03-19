using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json;

namespace Insight.BuildingBlocks.Serialization
{
    public sealed class SystemTextDateOnlyJsonConverter : System.Text.Json.Serialization.JsonConverter<DateOnly>
    {
        public const string DATEONLY_DATE_FORMAT = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString()!, DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStringValue(value.ToString(DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture));
        }
    }

    public class NewtonsoftDateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return DateOnly.ParseExact((string)reader.Value!, SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteValue(value.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture));
        }
    }
}
