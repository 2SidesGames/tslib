using Newtonsoft.Json;
using TSLib.SaveSystem.FileData;

namespace TSLib.SaveSystem.FileHandler
{
    public interface IFileOperator
    {
        public void SaveFile<T>(T gameData, bool overwrite) where T : TS_GameData;

        public T LoadFile<T>(string fileName, JsonSerializerSettings settings) where T : TS_GameData;

        public void DeleteFile(string fileName);

        public void DeleteAllFiles();
    }
}

