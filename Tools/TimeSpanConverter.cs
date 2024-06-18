using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tools
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            string timeSpanStr = reader.GetString();
            if (string.IsNullOrEmpty(timeSpanStr))
            {
                return TimeSpan.Zero;
            }

            try
            {
                // Chuyển đổi từ chuỗi "01:30:00" sang TimeSpan
                return TimeSpan.ParseExact(timeSpanStr, "hh\\:mm\\:ss", null);
            }
            catch (FormatException)
            {
                throw new JsonException($"Invalid TimeSpan format(hh/mm/ss): {timeSpanStr}");
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"hh\:mm\:ss"));
        }
    }
}
