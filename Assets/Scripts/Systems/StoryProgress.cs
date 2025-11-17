using System.Collections.Generic;

namespace SlimeLab.Systems
{
    public class StoryProgress
    {
        private HashSet<string> _milestones;
        private Dictionary<string, int> _progressValues;

        public StoryProgress()
        {
            _milestones = new HashSet<string>();
            _progressValues = new Dictionary<string, int>();
        }

        public void SetMilestone(string milestone)
        {
            _milestones.Add(milestone);
        }

        public bool HasMilestone(string milestone)
        {
            return _milestones.Contains(milestone);
        }

        public void SetProgressValue(string key, int value)
        {
            _progressValues[key] = value;
        }

        public int GetProgressValue(string key)
        {
            return _progressValues.ContainsKey(key) ? _progressValues[key] : 0;
        }

        public void IncrementProgressValue(string key, int amount)
        {
            int currentValue = GetProgressValue(key);
            SetProgressValue(key, currentValue + amount);
        }

        public IReadOnlyList<string> GetAllMilestones()
        {
            return new List<string>(_milestones);
        }
    }
}
