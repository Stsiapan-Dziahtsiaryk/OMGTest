using System;
using CodeBase.Infrastructure.Extensions;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.Environment.View
{
    public class BalloonView : MonoBehaviour
    {
        public class Pool : MonoPool<BalloonView>
        {
            public Pool(
                LifetimeScope container,
                BalloonView prefab,
                int maxInstances = 10,
                string parentName = "Parent of Pool")
                : base(container, prefab, maxInstances, parentName)
            {
            }
        }

        [SerializeField] private SpriteRenderer _graphic;
        
        [Header("Settings")]
        [SerializeField] private float _minDuration = 1f;
        [SerializeField] private float _maxDuration = 10f;
        [Space]
        [SerializeField] private float _minAmplitude = 1f;
        [SerializeField] private float _maxAmplitude = 10f;
        [Space]
        [SerializeField] private float _minFrequency = 1f;
        [SerializeField] private float _maxFrequency = 10f;

        private float speed = 1f;
        private float amplitude = 1f;
        private float frequency = 1f;
        
        private Tween _tween;

        public void Spawn(
            Vector2 at,
            float to,
            Sprite skin,
            Action<BalloonView> callback = null)
        {
            if(skin != null)
                _graphic.sprite = skin;
            transform.position = at;
            
            amplitude = Random.Range(_minAmplitude, _maxAmplitude);
            speed = Random.Range(_minDuration, _maxDuration);
            frequency = Random.Range(_minFrequency, _maxFrequency);

            _tween = transform
                .DOBlendableMoveBy(new Vector3(to, -amplitude, 0), speed)
                .SetEase(Ease.Linear)
                .OnComplete(() => callback?.Invoke(this));
            
            transform
                .DOBlendableMoveBy(
                    new Vector3(0, amplitude, 0),
                    frequency)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            _tween.Kill();
        }
    }
}