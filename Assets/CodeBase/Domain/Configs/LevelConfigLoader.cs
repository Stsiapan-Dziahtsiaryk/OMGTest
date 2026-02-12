using System;
using System.IO;
using CodeBase.Gameplay.Field.Config;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.Domain.Configs
{
    public class LevelConfigLoader
    {
        private const string FILE_NAME = "Level_";
        private readonly ISerializer _serializer = new JsonSerializer();

        public LevelData GetData(int id)
        {
            string json = LoadFromStreamingAssets(id).GetAwaiter().GetResult();
            return _serializer.Deserialize<LevelData>(json);
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