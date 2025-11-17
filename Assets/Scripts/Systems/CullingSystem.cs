using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class CullingSystem
    {
        public int RegisteredObjectCount => _objects.Count;

        private Dictionary<object, ObjectPosition> _objects;

        public CullingSystem()
        {
            _objects = new Dictionary<object, ObjectPosition>();
        }

        public void RegisterObject(object obj, float x, float y)
        {
            _objects[obj] = new ObjectPosition(x, y);
        }

        public void UnregisterObject(object obj)
        {
            _objects.Remove(obj);
        }

        public void UpdateObjectPosition(object obj, float x, float y)
        {
            if (_objects.ContainsKey(obj))
            {
                _objects[obj] = new ObjectPosition(x, y);
            }
        }

        public List<object> GetVisibleObjects(float viewportX, float viewportY, float viewportWidth, float viewportHeight)
        {
            var visible = new List<object>();

            foreach (var kvp in _objects)
            {
                var pos = kvp.Value;

                if (pos.X >= viewportX && pos.X <= viewportX + viewportWidth &&
                    pos.Y >= viewportY && pos.Y <= viewportY + viewportHeight)
                {
                    visible.Add(kvp.Key);
                }
            }

            return visible;
        }

        private class ObjectPosition
        {
            public float X { get; private set; }
            public float Y { get; private set; }

            public ObjectPosition(float x, float y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
