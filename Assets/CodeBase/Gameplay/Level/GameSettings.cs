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
    }
}