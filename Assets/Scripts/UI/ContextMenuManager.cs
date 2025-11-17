using System;
using System.Collections.Generic;

namespace SlimeLab.UI
{
    public class ContextMenuManager
    {
        public bool IsMenuVisible { get; private set; }
        public ContextMenu CurrentMenu { get; private set; }

        private Dictionary<string, Func<object, ContextMenu>> _menuBuilders;

        public ContextMenuManager()
        {
            IsMenuVisible = false;
            CurrentMenu = null;
            _menuBuilders = new Dictionary<string, Func<object, ContextMenu>>();
        }

        public void ShowMenu(ContextMenu menu)
        {
            CurrentMenu = menu;
            IsMenuVisible = true;
        }

        public void HideMenu()
        {
            CurrentMenu = null;
            IsMenuVisible = false;
        }

        public void RegisterMenuBuilder(string targetType, Func<object, ContextMenu> builder)
        {
            _menuBuilders[targetType] = builder;
        }

        public ContextMenu GetMenuForTarget(string targetType, object target)
        {
            if (_menuBuilders.ContainsKey(targetType))
            {
                return _menuBuilders[targetType](target);
            }

            return null;
        }
    }
}
