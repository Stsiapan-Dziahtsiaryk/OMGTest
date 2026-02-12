using CodeBase.Domain.Configs;
using CodeBase.Domain.Database.Data;
using UnityEngine;

namespace CodeBase.Domain.Database.Sheets
{
    public class PlayerSheet : ISaveSheet<PlayerSnapshot>
    {
        private static string KEY = "Player";
        private ISerializer _serializer = new JsonSerializer();

        public PlayerSnapshot Data { get; private set; }

        public void Load()
        {
            if (PlayerPrefs.HasKey(KEY))
            {
                string json = PlayerPrefs.GetString(KEY);
                Data = _serializer.Deserialize<PlayerSnapshot>(json);
            }
        }

        public void Save(PlayerSnapshot data)
        {
            string compressedJson = _serializer.Serialize(data);           
            PlayerPrefs.SetString(KEY, compressedJson);
            PlayerPrefs.Save();
        }
    }
}