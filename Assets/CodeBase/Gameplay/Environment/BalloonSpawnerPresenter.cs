using System;
using CodeBase.Gameplay.Environment.View;
using CodeBase.Gameplay.MVP;
using UnityEngine;

namespace CodeBase.Gameplay.Environment
{
    public class BalloonSpawnerPresenter : PresenterBase<BalloonSpawnerView>
    {
        private readonly BalloonView.Pool _pool;
        private readonly BalloonSpawner _spawner;
        
        public BalloonSpawnerPresenter(
            BalloonSpawnerView view,
            BalloonView.Pool pool, 
            BalloonSpawner spawner) 
            : base(view)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
        }

        protected override void OnAttach()
        {
            float height = Camera.main.orthographicSize; 
            float width = height * Camera.main.aspect;

            _spawner.Spawned += OnSpawn;
            _spawner.Start(new Vector2(width, height));
        }

        protected override void OnDetach()
        {
            _spawner.Spawned -= OnSpawn;
            _pool.DespawnAll();
        }

        private void OnSpawn(BalloonDto data)
        {
            var balloon = _pool.Spawn();
            balloon.transform.SetParent(View.transform, false);

            float delta =  data.StartPoint.x * (-2);
            balloon.Spawn(data.StartPoint, delta, data.Skin, OnDespawn);
        }
        
        private void OnDespawn(BalloonView instance)
        {
            _pool.Despawn(instance);
            _spawner.Return();
        }
    }
}