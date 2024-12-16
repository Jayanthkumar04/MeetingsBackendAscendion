using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetingsBackendAscendion.CustomConverter
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeString = reader.GetString();
            return TimeOnly.Parse(timeString);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("HH:mm"));
        }
    }
    // Method to read and convert string to TimeOnly
    //public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //{
    //    var timeString = reader.GetString(); // Get string from JSON (expected in HH:mm format)
    //    if (string.IsNullOrEmpty(timeString))
    //    {
    //        throw new JsonException("Invalid time format.");
    //    }

    //    return TimeOnly.ParseExact(timeString, "HH:mm"); // Parse time in HH:mm format
    //}

    //// Method to write TimeOnly object back to JSON as string
    //public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    //{
    //    writer.WriteStringValue(value.ToString("HH:mm")); // Write TimeOnly as string in HH:mm format
    //}
    //    }
}
