using System.Collections.Generic;

namespace SlimeLab.UI
{
    public class DragDropItem
    {
        public DragDropType Type { get; private set; }

        private object _data;
        private Dictionary<string, object> _metadata;

        public DragDropItem(object data, DragDropType type)
        {
            _data = data;
            Type = type;
            _metadata = new Dictionary<string, object>();
        }

        public T GetData<T>()
        {
            return (T)_data;
        }

        public void SetMetadata(string key, object value)
        {
            _metadata[key] = value;
        }

        public object GetMetadata(string key)
        {
            return _metadata.ContainsKey(key) ? _metadata[key] : null;
        }
    }
}
