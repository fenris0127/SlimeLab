using System;

namespace SlimeLab.UI
{
    public class ContextMenuAction
    {
        public string ID { get; private set; }
        public string Label { get; private set; }
        public string Icon { get; private set; }
        public bool IsEnabled { get; private set; }

        public event Action OnExecute;

        public ContextMenuAction(string id, string label, string icon = "")
        {
            ID = id;
            Label = label;
            Icon = icon;
            IsEnabled = true;
        }

        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        public void Execute()
        {
            if (!IsEnabled)
            {
                return;
            }

            OnExecute?.Invoke();
        }
    }
}
