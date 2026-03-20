using Newtonsoft.Json;

namespace TSLib.SaveSystem.FileHandler
{
    public interface ISerializer
    {
        public abstract string Extension { get; }

        public string Serialize<T>(T objeto);

        public T Deserialize<T>(string json, JsonSerializerSettings settings);
    }
}
