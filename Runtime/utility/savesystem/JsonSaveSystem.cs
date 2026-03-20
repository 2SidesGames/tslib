using Newtonsoft.Json;
using TSLib.SaveSystem.FileData;
using TSLib.SaveSystem.FileHandler;
using TSLib.Utility.Management.Component.Capabilities;

namespace TSLib.SaveSystem
{
    public class JsonSaveSystem : ControllerBase
    {
        private FileOperator _fileOperator;
        private SaveDataBase _saveData;

        public override void Initialize()
        {
            var serializer = new FileHandler.JsonSerializer();
            _fileOperator = new FileOperator(serializer);
            base.Initialize();
        }

        public void Save()
        {
            _fileOperator.SaveFile(_saveData, true);
        }

        public void Load(string fileName, JsonSerializerSettings settings)
        {
            _saveData = _fileOperator.LoadFile(fileName, settings);
        }

        public void Delete(string fileName)
        {
            _fileOperator.DeleteFile(fileName);
        }
    }
}