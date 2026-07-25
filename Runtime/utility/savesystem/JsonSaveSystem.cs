using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SGLib.SaveSystem.FileData;
using SGLib.SaveSystem.FileHandler;
using SGLib.Utility.Management.Component.Capabilities;

namespace SGLib.SaveSystem
{
    public class JsonSaveSystem : SG_Component
    {
        public SG_GameData GameData { get; set; }

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

        public void Load<T>(string fileName, JsonSerializerSettings settings = null) where T : SG_GameData
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

        public async UniTask LoadAsync<T>(string fileName, JsonSerializerSettings settings = null, CancellationToken ct = default) where T : SG_GameData
        {
            GameData = await fileOperator.LoadFileAsync<T>(fileName, settings, ct);
        }
    }
}