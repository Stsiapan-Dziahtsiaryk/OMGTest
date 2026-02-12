using System;
using System.IO;
using CodeBase.Gameplay.Field.Config;
using CodeBase.Gameplay.Level;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using VContainer.Unity;

namespace CodeBase.Domain.Configs
{
    public class LevelConfigLoader : IInitializable
    {
        private const string FILE_NAME = "Level_";
        
        private readonly ISerializer _serializer = new JsonSerializer();
        
        private string[] _data;

        public LevelConfigLoader(GameSettings settings)
        {
            // Временное решение до появления RemoteConfig или другого способа получения данных с сервера
            _data = new string[settings.LevelAmount];
        }
        
        public async void Initialize()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                string json = await LoadFromStreamingAssets(i);
                _data[i] = json;
            }
        }
        
        public LevelData GetData(int id)
        {
            return _serializer.Deserialize<LevelData>(_data[id]);
        }
        
        private async UniTask<string> LoadFromStreamingAssets(int id)
        {
            string path = Path.Combine(Application.streamingAssetsPath, FILE_NAME + $"{id}.txt");
            if (path.Contains("://") || path.Contains("jar:"))
            {
                using var req = UnityWebRequest.Get(path);
                await req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success)
                    throw new Exception($"Error reading file: {req.error}");
                
                return req.downloadHandler.text;
            }
            else
            {
                return File.ReadAllText(path);
            }
        }

        
    }
}