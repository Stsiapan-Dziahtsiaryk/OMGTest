using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.Infrastructure.Extensions
{
     public abstract class MonoPool<T> : IStartable, IDisposable
        where T : MonoBehaviour
    {
        private readonly LifetimeScope _container;
        private readonly T _prefab;
        private readonly Queue<T> _instances;
        private readonly Transform _parent;
        private readonly int _maxInstances;
        private readonly List<T> _spawnedInstances;

        protected MonoPool(
            LifetimeScope container,
            T prefab,
            int maxInstances = 10,
            string parentName = "Parent of Pool")
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            _maxInstances = maxInstances;
            _instances = new Queue<T>(maxInstances);
            _spawnedInstances = new List<T>(maxInstances);
            _parent = new GameObject(parentName).transform;
        }

        public virtual T Spawn()
        {
            T instance = _instances.Dequeue();
            instance.gameObject.SetActive(true);
            _spawnedInstances.Add(instance);
            return instance;
        }

        public virtual void Despawn(T obj)
        {
            if (obj.gameObject.activeSelf)
                obj.transform.SetParent(_parent, false);
            obj.gameObject.SetActive(false);
            obj.transform.position = Vector3.zero;
            _spawnedInstances.Remove(obj);
            _instances.Enqueue(obj);
        }

        public void DespawnAll()
        {
            foreach (T instance in _spawnedInstances)
            {
                DespawnAll(instance);
            }

            _spawnedInstances.Clear();
        }
        
        protected virtual void DespawnAll(T instance)
        {
            if(instance == null) return;
            if (instance.gameObject.activeSelf)
                instance.transform.SetParent(_parent, false);
            instance.gameObject.SetActive(false);
            instance.transform.position = Vector3.zero;
            _instances.Enqueue(instance);
        }
        
        protected virtual void CreateInstances()
        {
            for (int i = 0; i < _maxInstances; i++)
            {
                T instance = _container.Container.Instantiate(_prefab, _parent);
                instance.gameObject.SetActive(false);
                _instances.Enqueue(instance);
            }
        }

        public void Start()
        {
            CreateInstances();
        }

        public void Dispose()
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                T obj = _instances.Dequeue();
                if (obj != null)
                    obj.gameObject.OnDestroyAsync();
            }

            if (_parent != null)
                _parent.OnDestroyAsync();
        }
    }
}