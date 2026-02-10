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

        private AnimatorOverrideController _overrideController;

        public event Action Destroyed;
        
        private void OnDisable()
        {
            _animator.SetTrigger("Alive");
        }

        public void Initialize(int layerId, AnimationClip[] clips)
        {
            SetLayer(layerId);
            
            if(clips == null)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            if( _overrideController == null)
            {
                _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
                _animator.runtimeAnimatorController = _overrideController;
            }
            
            _overrideController["Idle"] = clips[0];
            _overrideController["Destroy"] = clips[1];
        }
        
        public void SetBlock(int id, Action callback = null)
        {
            if(id < 0)
            {
                _graphic.sprite = null;
            } else 
                _graphic.sprite = _sprites[id];
        }

        public void DestroyBlock(Action callback = null)
        {
            // _graphic.sprite = null;
            // if(gameObject.activeSelf)
            //     StartCoroutine(Test(callback));
            if(_animator.enabled)
                _animator.SetTrigger("Destroy");
        }

        public void DestroyFinish()
        {
            Destroyed?.Invoke();
        }
        
        public void SetLayer(int layer)
        {
            _graphic.sortingOrder = layer;
        }

        private IEnumerator Test(Action callback)
        {
            yield return new WaitForSeconds(1);
            callback?.Invoke();
        }
    }
}