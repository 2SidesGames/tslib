using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TSLib.SaveSystem.FileData;
using TSLib.SaveSystem.FileHandler;
using TSLib.Utility.Management.Component.Capabilities;

namespace TSLib.SaveSystem
{
    public class JsonSaveSystem : TSComponent
    {
        public GameDataBase GameData { get; set; }

        private FileOperator _fileOperator;

        public override void Initialize()
        {
            var serializer = new FileHandler.JsonSerializer();
            _fileOperator = new FileOperator(serializer);
        }

        public void Save()
        {
            _fileOperator.SaveFile(GameData, true);
        }

        public void Load<T>(string fileName, JsonSerializerSettings settings = null) where T : GameDataBase
        {
            GameData = _fileOperator.LoadFile<T>(fileName, settings);
        }

        public void Delete(string fileName)
        {
            _fileOperator.DeleteFile(fileName);
        }

        public void DeleteAll()
        {
            _fileOperator.DeleteAllFiles();
        }

        public async UniTask SaveAsync(CancellationToken ct)
        {
            await _fileOperator.SaveFileAsync(GameData, true, ct);
        }

        public async UniTask LoadAsync<T>(string fileName, JsonSerializerSettings settings = null, CancellationToken ct = default) where T : GameDataBase
        {
            GameData = await _fileOperator.LoadFileAsync<T>(fileName, settings, ct);
        }
    }
}