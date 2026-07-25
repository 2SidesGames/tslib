using Newtonsoft.Json;
using SGLib.SaveSystem.FileData;

namespace SGLib.SaveSystem.FileHandler
{
    public interface IFileOperator
    {
        public void SaveFile<T>(T gameData, bool overwrite) where T : SG_GameData;

        public T LoadFile<T>(string fileName, JsonSerializerSettings settings) where T : SG_GameData;

        public void DeleteFile(string fileName);

        public void DeleteAllFiles();
    }
}

