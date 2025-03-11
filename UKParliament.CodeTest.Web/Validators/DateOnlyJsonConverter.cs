using System.Text.Json.Serialization;
using System.Text.Json;

namespace UKParliament.CodeTest.Web.Validators;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (DateOnly.TryParse(reader.GetString(), out var date))
        {
            return date;
        }

        throw new JsonException($"Invalid date format. Please use '{Format}'.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
