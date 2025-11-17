using System;

namespace SlimeLab.Systems
{
    public class TutorialStep
    {
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsComplete { get; private set; }
        public string HighlightTarget { get; private set; }
        public bool IsSkippable { get; private set; }

        private Action _action;

        public TutorialStep(string id, string title, string description)
        {
            ID = id;
            Title = title;
            Description = description;
            IsComplete = false;
            HighlightTarget = null;
            IsSkippable = false;
            _action = null;
        }

        public void Complete()
        {
            IsComplete = true;
        }

        public void SetAction(Action action)
        {
            _action = action;
        }

        public void ExecuteAction()
        {
            _action?.Invoke();
        }

        public void SetHighlightTarget(string target)
        {
            HighlightTarget = target;
        }

        public void SetSkippable(bool skippable)
        {
            IsSkippable = skippable;
        }
    }
}
