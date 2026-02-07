using UnityEngine;

namespace CodeBase.Gameplay.Field.Config
{
    [CreateAssetMenu(fileName = "Block", menuName = "Data/Configs/Create new Block", order = 51)]
    public class BlockConfig : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: Tooltip("Only for debug purposes")]
        [field: SerializeField] public string Name { get; private set; }
    }
}