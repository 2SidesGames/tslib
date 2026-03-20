using Newtonsoft.Json;
using System;
using System.IO;
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

        public void SaveFile(GameDataBase gameData, bool overwrite = true)
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

        public GameDataBase LoadFile(string fileName, JsonSerializerSettings settings = null)
        {
            string filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
            {
                throw new IOException($"{fileName}.{_serializer.Extension} doesn't exist.");
            }
            return _serializer.Deserialize<GameDataBase>(File.ReadAllText(filePath), settings);
        }

        public void DeleteFile(string fileName)
        {
            string filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
            {
                throw new IOException($"{fileName}.{_serializer.Extension} doesn't exist.");
            }

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

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_directoryPath, string.Concat(fileName, _serializer.Extension));
        }
    }
}


