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

        [Header("Subscription Events")]
        [SerializeField] private VoidChannel_So onDoSave;
        [SerializeField] private StringChannel_So onDoLoad;
        [SerializeField] private StringChannel_So onDoDelete;
        [SerializeField] private VoidChannel_So onDoDeleteAll;

        private FileOperator _fileOperator;

        public override void Initialize()
        {
            var serializer = new FileHandler.JsonSerializer();
            _fileOperator = new FileOperator(serializer);
        }

        public override void Activate()
        {
            onDoSave.Subscribe(Save);
            onDoLoad.Subscribe(Load);
            onDoDelete.Subscribe(Delete);
            onDoDeleteAll.Subscribe(DeleteAll);
        }

        public override void Deactivate()
        {
            onDoSave.Unsubscribe(Save);
            onDoLoad.Unsubscribe(Load);
            onDoDelete.Unsubscribe(Delete);
            onDoDeleteAll.Unsubscribe(DeleteAll);
        }

        public void SetGameData(GameDataBase gameData)
        {
            if (gameData == null)
                throw new ArgumentNullException(
                    "(missing) there is no game data");

            GameData = gameData;
        }

        public void Save()
        {
            _fileOperator.SaveFile(GameData, true);
            onSave.TriggerEvent();
        }

        public void Load(string fileName)
        {
            GameData = _fileOperator.LoadFile(fileName);
            onLoad.TriggerEvent();
        }

        public void Load(string fileName, JsonSerializerSettings settings)
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

        public async UniTask LoadAsync(string fileName, CancellationToken ct, JsonSerializerSettings settings = null)
        {
            GameData = await _fileOperator.LoadFileAsync(fileName, settings, ct);
            onLoad.TriggerEvent();
        }
    }
}