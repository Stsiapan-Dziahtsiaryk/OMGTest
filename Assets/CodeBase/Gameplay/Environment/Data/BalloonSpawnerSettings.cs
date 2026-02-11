using UnityEngine;

namespace CodeBase.Gameplay.Environment
{
    [CreateAssetMenu(fileName = "Balloon Spawner Settings", menuName = "Data/Settings/Environment/Balloon Spawner", order = 51)]
    public class BalloonSpawnerSettings : ScriptableObject
    {
        [field: SerializeField] public int Amount { get; private set; }
        
        [field: SerializeField] public int MinTimeInterval { get; private set; } 
        [field: SerializeField] public int MaxTimeInterval { get; private set; } 
        [field: SerializeField] public float HorizontalOffset { get; private set; }
        [field: SerializeField] public float VerticalOffset { get; private set; }
        
        [field: SerializeField] public Sprite[] Skins { get; private set; }
    } 
}