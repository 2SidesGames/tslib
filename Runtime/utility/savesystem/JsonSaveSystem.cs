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
    public class JsonSaveSystem : TS_Component
    {
        public TS_GameData GameData { get; set; }

        private FileOperator fileOperator;

        public override void Initialize()
        {
            var serializer = new FileHandler.JsonSerializer();
            fileOperator = new FileOperator(serializer);
        }

        public void Save()
        {
            fileOperator.SaveFile(GameData, true);
        }

        public void Load<T>(string fileName, JsonSerializerSettings settings = null) where T : TS_GameData
        {
            GameData = fileOperator.LoadFile<T>(fileName, settings);
        }

        public void Delete(string fileName)
        {
            fileOperator.DeleteFile(fileName);
        }

        public void DeleteAll()
        {
            fileOperator.DeleteAllFiles();
        }

        public async UniTask SaveAsync(CancellationToken ct)
        {
            await fileOperator.SaveFileAsync(GameData, true, ct);
        }

        public async UniTask LoadAsync<T>(string fileName, JsonSerializerSettings settings = null, CancellationToken ct = default) where T : TS_GameData
        {
            GameData = await fileOperator.LoadFileAsync<T>(fileName, settings, ct);
        }
    }
}