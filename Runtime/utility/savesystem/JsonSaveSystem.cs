using System;
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

        [SerializeField] private VoidChannel_So onSave;
        [SerializeField] private VoidChannel_So onLoad;
        [SerializeField] private VoidChannel_So onDelete;

        private FileOperator _fileOperator;

        public override void Initialize()
        {
            var serializer = new FileHandler.JsonSerializer();
            _fileOperator = new FileOperator(serializer);
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
    }
}