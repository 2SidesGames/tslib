using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using TSLib.SaveSystem.FileData;
using UnityEngine;

namespace TSLib.SaveSystem.FileHandler
{
    public class FileOperator : IFileOperator
    {
        private readonly ISerializer _serializer;

        private readonly string _directoryPath;

        public FileOperator(ISerializer serializer)
        {
            _serializer = serializer;
            _directoryPath = Application.persistentDataPath;
        }

        public void SaveFile<T>(T gameData, bool overwrite = true) where T : GameDataBase
        {
            if (gameData == null) throw new ArgumentNullException(
                "(missing) there is no data to save.");

            string filePath = GetFilePath(gameData.FileName);

            if (!overwrite && File.Exists(filePath))
            {
                throw new IOException(
                    $"'{gameData.FileName}.{_serializer.Extension}' already exists.");
            }
            File.WriteAllText(filePath, _serializer.Serialize(gameData));
        }

        public T LoadFile<T>(string fileName, JsonSerializerSettings settings = null) where T : GameDataBase
        {
            string filePath = GetFilePath(fileName);

            if (!File.Exists(filePath)) return null;

            return _serializer.Deserialize<T>(File.ReadAllText(filePath), settings);
        }

        public void DeleteFile(string fileName)
        {
            string filePath = GetFilePath(fileName);

            if (!File.Exists(filePath)) return;

            File.Delete(filePath);
        }

        public void DeleteAllFiles()
        {
            var files = Directory.GetFiles(_directoryPath);

            foreach (var fileName in files)
            {
                DeleteFile(fileName);
            }
        }

        public async UniTask SaveFileAsync<T>(T gameData, bool overwrite = true, CancellationToken ct = default) where T : GameDataBase
        {
            if (gameData == null)
                throw new ArgumentNullException(nameof(gameData), "There is no data to save.");

            string filePath = GetFilePath(gameData.FileName);

            if (!overwrite && File.Exists(filePath))
                throw new IOException($"'{gameData.FileName}.{_serializer.Extension}' already exists.");

            await UniTask.SwitchToThreadPool();

            try
            {
                ct.ThrowIfCancellationRequested();

                string json = _serializer.Serialize(gameData);

                ct.ThrowIfCancellationRequested();

                await File.WriteAllTextAsync(filePath, json, ct);
            }
            finally
            {
                await UniTask.SwitchToMainThread();
            }
        }

        public async UniTask<T> LoadFileAsync<T>(string fileName, JsonSerializerSettings settings = null, CancellationToken ct = default) where T : GameDataBase
        {
            string filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
                return default;

            string json = await File.ReadAllTextAsync(filePath, ct);

            await UniTask.SwitchToThreadPool();

            try
            {
                ct.ThrowIfCancellationRequested();
                return _serializer.Deserialize<T>(json, settings);
            }
            finally
            {
                await UniTask.SwitchToMainThread();
            }
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_directoryPath, string.Concat(fileName, _serializer.Extension));
        }
    }
}


