using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _graphic;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private Animator _animator;
        
        public void SetBlock(int id, Action callback = null)
        {
            if(id < 0)
            {
                _graphic.sprite = null;
            } else 
                _graphic.sprite = _sprites[id];
            if(gameObject.activeSelf)
                StartCoroutine(Test(callback));
        }

        public void DestroyBlock(Action callback = null)
        {
            _graphic.sprite = null;
            if(gameObject.activeSelf)
                StartCoroutine(Test(callback));
        }
        
        public void SetLayer(int layer)
        {
            _graphic.sortingOrder = layer;
        }

        private IEnumerator Test(Action callback)
        {
            yield return new WaitForSeconds(3);
            callback?.Invoke();
        }
    }
}