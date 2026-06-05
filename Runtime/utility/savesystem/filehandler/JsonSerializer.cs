using Newtonsoft.Json;

namespace TSLib.SaveSystem.FileHandler
{
    public class JsonSerializer : ISerializer
    {
        public string Extension => ".json";

        private readonly JsonSerializerSettings settings;

        public JsonSerializer()
        {
            settings = new()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public string Serialize<T>(T serializableObject)
        {
            return JsonConvert.SerializeObject(serializableObject, Formatting.Indented, settings);
        }

        public T Deserialize<T>(string json, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, settings ?? this.settings);
        }
    }
}

