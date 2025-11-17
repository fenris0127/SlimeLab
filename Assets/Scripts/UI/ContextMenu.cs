using System.Collections.Generic;

namespace SlimeLab.UI
{
    public class ContextMenu
    {
        public int ActionCount => _actions.Count;

        private List<ContextMenuAction> _actions;

        public ContextMenu()
        {
            _actions = new List<ContextMenuAction>();
        }

        public void AddAction(ContextMenuAction action)
        {
            _actions.Add(action);
        }

        public void RemoveAction(ContextMenuAction action)
        {
            _actions.Remove(action);
        }

        public void ClearActions()
        {
            _actions.Clear();
        }

        public List<ContextMenuAction> GetActions()
        {
            return new List<ContextMenuAction>(_actions);
        }
    }
}
