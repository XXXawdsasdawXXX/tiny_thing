using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game.Infrastructure.Save
{
    public sealed class JsonSaveSerializer : ISaveSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, Options);
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options)!;
        }
    }
}
