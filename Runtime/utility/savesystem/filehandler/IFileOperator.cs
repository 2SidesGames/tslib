using Newtonsoft.Json;
using TSLib.SaveSystem.FileData;

namespace TSLib.SaveSystem.FileHandler
{
    public interface IFileOperator
    {
        public void SaveFile(SaveDataBase gameData, bool overwrite);

        public SaveDataBase LoadFile(string fileName, JsonSerializerSettings settings);

        public void DeleteFile(string fileName);

        public void DeleteAllFiles();
    }
}

