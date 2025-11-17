using System;
using System.Collections.Generic;

namespace SlimeLab.UI
{
    public class DropZone
    {
        public string ID { get; private set; }
        public bool IsEnabled { get; private set; }

        public event Action<DragDropItem> OnDrop;

        private HashSet<DragDropType> _acceptedTypes;
        private Func<DragDropItem, bool> _customValidator;

        public DropZone(string id, DragDropType acceptedType)
        {
            ID = id;
            IsEnabled = true;
            _acceptedTypes = new HashSet<DragDropType> { acceptedType };
            _customValidator = null;
        }

        public void AddAcceptedType(DragDropType type)
        {
            _acceptedTypes.Add(type);
        }

        public bool CanAccept(DragDropType type)
        {
            return _acceptedTypes.Contains(type);
        }

        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        public void SetCustomValidator(Func<DragDropItem, bool> validator)
        {
            _customValidator = validator;
        }

        public bool ValidateDrop(DragDropItem item)
        {
            if (!IsEnabled)
            {
                return false;
            }

            if (!CanAccept(item.Type))
            {
                return false;
            }

            if (_customValidator != null)
            {
                return _customValidator(item);
            }

            return true;
        }

        public void HandleDrop(DragDropItem item)
        {
            if (!ValidateDrop(item))
            {
                return;
            }

            OnDrop?.Invoke(item);
        }
    }
}
