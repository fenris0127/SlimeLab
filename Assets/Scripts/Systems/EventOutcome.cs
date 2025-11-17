using System.Collections.Generic;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class EventOutcome
    {
        public string Description { get; private set; }

        private Dictionary<ResourceType, int> _resourceRewards;

        public EventOutcome(string description = "")
        {
            Description = description;
            _resourceRewards = new Dictionary<ResourceType, int>();
        }

        public void AddResourceReward(ResourceType resourceType, int amount)
        {
            if (_resourceRewards.ContainsKey(resourceType))
            {
                _resourceRewards[resourceType] += amount;
            }
            else
            {
                _resourceRewards[resourceType] = amount;
            }
        }

        public Dictionary<ResourceType, int> GetResourceRewards()
        {
            return new Dictionary<ResourceType, int>(_resourceRewards);
        }

        public void ApplyToInventory(ResourceInventory inventory)
        {
            foreach (var reward in _resourceRewards)
            {
                if (reward.Value > 0)
                {
                    inventory.AddResource(new Resource(reward.Key, reward.Value));
                }
                else if (reward.Value < 0)
                {
                    // Negative reward = cost
                    inventory.ConsumeResource(reward.Key, -reward.Value);
                }
            }
        }
    }
}
