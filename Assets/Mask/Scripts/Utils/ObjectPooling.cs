using System.Collections.Generic;
using UnityEngine;

namespace HorrorGameJam
{
    public class ObjectPooling<T> where T : Component
    {
        private readonly T _preset;
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly Transform _parent;
        private readonly bool _isExtended;
        public ObjectPooling(T preset, int amount, bool isExtended, Transform parent)
        {
            _preset = preset;
            _parent = parent;
            _isExtended = isExtended;
            for (int i = 0; i < amount; i++)
                AddObjectToPool();
        }
        private T AddObjectToPool()
        {
            var obj = Object.Instantiate(_preset, _parent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
            return obj;
        }
        public T Get()
        {
            if (_pool.Count == 0 && _isExtended)
                AddObjectToPool();

            var obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
