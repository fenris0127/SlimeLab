using System;
using System.Collections.Generic;

namespace SlimeLab.Systems
{
    public class ObjectPool<T> where T : class
    {
        public int Capacity { get; private set; }
        public int AvailableCount => _availableObjects.Count;
        public int ActiveCount => _activeObjects.Count;

        private Stack<T> _availableObjects;
        private HashSet<T> _activeObjects;
        private Func<T> _factory;

        public ObjectPool(Func<T> factory, int initialCapacity)
        {
            _factory = factory;
            Capacity = initialCapacity;
            _availableObjects = new Stack<T>(initialCapacity);
            _activeObjects = new HashSet<T>();

            // Prewarm pool
            for (int i = 0; i < initialCapacity; i++)
            {
                _availableObjects.Push(_factory());
            }
        }

        public T Get()
        {
            T obj;

            if (_availableObjects.Count > 0)
            {
                obj = _availableObjects.Pop();
            }
            else
            {
                // Expand pool
                obj = _factory();
                Capacity++;
            }

            _activeObjects.Add(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (_activeObjects.Remove(obj))
            {
                _availableObjects.Push(obj);
            }
        }

        public void Clear()
        {
            foreach (var obj in _activeObjects)
            {
                _availableObjects.Push(obj);
            }
            _activeObjects.Clear();
        }
    }
}
