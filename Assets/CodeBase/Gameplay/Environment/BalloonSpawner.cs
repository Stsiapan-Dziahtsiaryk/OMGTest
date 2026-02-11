using System;
using UnityEngine;
using VContainer.Unity;
using Random = System.Random;

namespace CodeBase.Gameplay.Environment
{
    public class BalloonSpawner : IFixedTickable, IDisposable
    {
        private readonly BalloonSpawnerSettings _settings;
        private readonly Random _random = new Random();

        private int _availableCount = 0;
        private float _interval = 0;
        
        private float _width = 0;
        private float _height = 0;

        public BalloonSpawner(BalloonSpawnerSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public event Action<BalloonDto> Spawned;
        
        public void Start(Vector2 screenSize)
        {
            _availableCount = _settings.Amount;
            _interval = _random.Next(_settings.MinTimeInterval, _settings.MaxTimeInterval);
            
            _width = screenSize.x  + _settings.HorizontalOffset;
            _height = screenSize.y - _settings.VerticalOffset;
        }

        public void FixedTick()
        {
            if (_interval <= 0)
                Spawn();
            
            _interval -= Time.fixedDeltaTime;
        }
       
        public void Return()
        {
            if (_availableCount >= _settings.Amount) return;
            _availableCount++;
        }

        private void Spawn()
        {
            if (_availableCount == 0) return;
            int spawn = _random.Next(0, _availableCount);
            if(spawn >= _settings.Amount)
            {
                _interval = _random.Next(_settings.MinTimeInterval, _settings.MaxTimeInterval);
                return;
            }
            
            _availableCount -= spawn;
            for (int i = 0; i < spawn; i++)
            {
                Spawned?.Invoke(CreateBalloon());
            }
            
            _interval = _random.Next(_settings.MinTimeInterval, _settings.MaxTimeInterval);
        }

        private BalloonDto CreateBalloon()
        {
            float y = UnityEngine.Random.Range(0, _height);
            
            int sign = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
            float x = sign * _width;
            
            int skinId = _random.Next(0, _settings.Skins.Length);

            return new BalloonDto(_settings.Skins[skinId], new Vector2(x, y));
        }

        public void Dispose()
        {
        }
    }
}