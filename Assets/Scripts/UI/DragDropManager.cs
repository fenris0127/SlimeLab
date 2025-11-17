using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.UI
{
    public class DragDropManager
    {
        public bool IsDragging { get; private set; }
        public DragDropItem CurrentDragItem { get; private set; }
        public int DropZoneCount => _dropZones.Count;

        private List<DropZone> _dropZones;

        public DragDropManager()
        {
            IsDragging = false;
            CurrentDragItem = null;
            _dropZones = new List<DropZone>();
        }

        public void StartDrag(DragDropItem item)
        {
            IsDragging = true;
            CurrentDragItem = item;
        }

        public void EndDrag()
        {
            IsDragging = false;
            CurrentDragItem = null;
        }

        public void CancelDrag()
        {
            IsDragging = false;
            CurrentDragItem = null;
        }

        public void RegisterDropZone(DropZone dropZone)
        {
            _dropZones.Add(dropZone);
        }

        public void UnregisterDropZone(DropZone dropZone)
        {
            _dropZones.Remove(dropZone);
        }

        public DropZone GetDropZone(string id)
        {
            return _dropZones.FirstOrDefault(z => z.ID == id);
        }
    }
}
