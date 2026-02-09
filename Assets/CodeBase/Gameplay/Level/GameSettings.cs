using CodeBase.Gameplay.Field.Config;
using UnityEngine;

namespace CodeBase.Gameplay.Level
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Data/Create Settings", order = 51)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public float StartYPosition { get; private set; }
        [field: SerializeField] public float HorizontalMargin { get; private set; }
        [field: SerializeField] public float ReferenceSizeBlock { get; private set; }
        [field: SerializeField] public float ReferenceHMargin { get; private set; }
        
        // ToDo: we should get it from the json file
        [field: SerializeField] public LevelConfig[] LevelConfigs { get; private set; }
        [field: SerializeField] public BlockConfig[] BlockConfigs { get; private set; }
    }
}