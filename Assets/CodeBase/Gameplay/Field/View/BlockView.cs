using CodeBase.Infrastructure.Extensions;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.Gameplay.Field
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _graphic;
        [SerializeField] private Sprite[] _sprites;

        public void SetBlock(int id)
        {
            if(id < 0)
            {
                _graphic.sprite = null;
                return;
            }
            _graphic.sprite = _sprites[id];
        }

        public void SetLayer(int layer)
        {
            _graphic.sortingOrder = layer;
        }
    }
}