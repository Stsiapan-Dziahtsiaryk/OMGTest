using CodeBase.Domain.Configs;
using CodeBase.Domain.Database.Data;
using UnityEngine;

namespace CodeBase.Domain.Database.Sheets
{
    public class LevelSheet : ISaveSheet<LevelSnapshot>
    {
        private const string KEY = "Session";
        private ISerializer _serializer = new JsonSerializer();

        public LevelSnapshot Data { get; private set; }
        
        public void Load()
        {
            if (PlayerPrefs.HasKey(KEY))
            {
                string json = PlayerPrefs.GetString(KEY);
                Data = _serializer.Deserialize<LevelSnapshot>(json);
            }
        }


        public void Save(LevelSnapshot data)
        {
            string compressedJson = _serializer.Serialize(data); 
            PlayerPrefs.SetString(KEY, compressedJson);
            PlayerPrefs.Save();
        }
    }
}