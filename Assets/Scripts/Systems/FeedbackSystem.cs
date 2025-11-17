using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class FeedbackSystem
    {
        public int RegisteredFeedbackCount { get; private set; }
        public bool IsEnabled { get; private set; }

        public event Action<string, FeedbackType, string> OnFeedbackTriggered;

        private Dictionary<string, List<FeedbackEntry>> _feedbacks;

        public FeedbackSystem()
        {
            _feedbacks = new Dictionary<string, List<FeedbackEntry>>();
            RegisteredFeedbackCount = 0;
            IsEnabled = true;
        }

        public void RegisterFeedback(string action, FeedbackType type, string data)
        {
            if (!_feedbacks.ContainsKey(action))
            {
                _feedbacks[action] = new List<FeedbackEntry>();
            }

            _feedbacks[action].Add(new FeedbackEntry(type, data));
            RegisteredFeedbackCount++;
        }

        public void TriggerFeedback(string action)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (!_feedbacks.ContainsKey(action))
            {
                return;
            }

            foreach (var feedback in _feedbacks[action])
            {
                OnFeedbackTriggered?.Invoke(action, feedback.Type, feedback.Data);
            }
        }

        public List<FeedbackEntry> GetFeedbacks(string action)
        {
            return _feedbacks.ContainsKey(action) ? new List<FeedbackEntry>(_feedbacks[action]) : new List<FeedbackEntry>();
        }

        public void ClearFeedback(string action)
        {
            if (_feedbacks.ContainsKey(action))
            {
                RegisteredFeedbackCount -= _feedbacks[action].Count;
                _feedbacks.Remove(action);
            }
        }

        public List<string> GetAllActions()
        {
            return new List<string>(_feedbacks.Keys);
        }

        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        public class FeedbackEntry
        {
            public FeedbackType Type { get; private set; }
            public string Data { get; private set; }

            public FeedbackEntry(FeedbackType type, string data)
            {
                Type = type;
                Data = data;
            }
        }
    }
}
