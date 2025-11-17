using System;
using System.Collections.Generic;

namespace SlimeLab.Systems
{
    public class StoryEvent
    {
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsTriggered { get; private set; }
        public List<string> MilestoneRequirements { get; private set; }
        public Dictionary<string, int> ProgressRequirements { get; private set; }
        public List<string> ContentUnlocks { get; private set; }

        public StoryEvent(string id, string title, string description)
        {
            ID = id;
            Title = title;
            Description = description;
            IsTriggered = false;
            MilestoneRequirements = new List<string>();
            ProgressRequirements = new Dictionary<string, int>();
            ContentUnlocks = new List<string>();
        }

        public void AddMilestoneRequirement(string milestone)
        {
            MilestoneRequirements.Add(milestone);
        }

        public void AddProgressRequirement(string key, int requiredValue)
        {
            ProgressRequirements[key] = requiredValue;
        }

        public void AddContentUnlock(string contentID)
        {
            ContentUnlocks.Add(contentID);
        }

        public bool IsAvailable(StoryProgress progress)
        {
            // Check all milestone requirements
            foreach (var milestone in MilestoneRequirements)
            {
                if (!progress.HasMilestone(milestone))
                {
                    return false;
                }
            }

            // Check all progress requirements
            foreach (var requirement in ProgressRequirements)
            {
                int currentValue = progress.GetProgressValue(requirement.Key);
                if (currentValue < requirement.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public void Trigger()
        {
            if (IsTriggered)
            {
                throw new InvalidOperationException("Story event has already been triggered");
            }

            IsTriggered = true;
        }

        public void ApplyUnlocks(ContentUnlockManager manager)
        {
            if (!IsTriggered)
            {
                throw new InvalidOperationException("Story event must be triggered before applying unlocks");
            }

            foreach (var contentID in ContentUnlocks)
            {
                manager.UnlockContent(contentID);
            }
        }
    }
}
