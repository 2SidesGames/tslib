using Newtonsoft.Json;
using TSLib.SaveSystem.FileData;

namespace TSLib.SaveSystem.FileHandler
{
    public interface IFileOperator
    {
        public void SaveFile(GameDataBase gameData, bool overwrite);

        public GameDataBase LoadFile(string fileName, JsonSerializerSettings settings);

        public void DeleteFile(string fileName);

        public void DeleteAllFiles();
    }
}

