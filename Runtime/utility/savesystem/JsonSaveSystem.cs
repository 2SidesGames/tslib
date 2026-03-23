using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TSLib.SaveSystem.FileData;
using TSLib.SaveSystem.FileHandler;
using TSLib.Utility.Management.Component.Capabilities;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

namespace TSLib.SaveSystem
{
    public class JsonSaveSystem : ComponentBase
    {
        public GameDataBase GameData { get; private set; }

        [Header("Trigger Events")]
        [SerializeField] private VoidChannel_So onSave;
        [SerializeField] private VoidChannel_So onLoad;
        [SerializeField] private VoidChannel_So onDelete;
        [SerializeField] private VoidChannel_So onDeleteAll;

        private FileOperator _fileOperator;

        public override void Initialize()
        {
            var serializer = new FileHandler.JsonSerializer();
            _fileOperator = new FileOperator(serializer);
        }

        public void Save()
        {
            _fileOperator.SaveFile(GameData, true);
            onSave.TriggerEvent();
        }

        public void Load(string fileName, JsonSerializerSettings settings = null)
        {
            GameData = _fileOperator.LoadFile(fileName, settings);
            onLoad.TriggerEvent();
        }

        public void Delete(string fileName)
        {
            _fileOperator.DeleteFile(fileName);
            onDelete.TriggerEvent();
        }

        public void DeleteAll()
        {
            _fileOperator.DeleteAllFiles();
            onDeleteAll.TriggerEvent();
        }

        public async UniTask SaveAsync(CancellationToken ct)
        {
            await _fileOperator.SaveFileAsync(GameData, true, ct);
            onSave.TriggerEvent();
        }

        public async UniTask LoadAsync(string fileName, JsonSerializerSettings settings = null, CancellationToken ct = default)
        {
            GameData = await _fileOperator.LoadFileAsync(fileName, settings, ct);

            if (GameData == null) return;

            onLoad.TriggerEvent();
        }
    }
}