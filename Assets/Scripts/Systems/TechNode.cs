using System;
using System.Collections.Generic;
using System.Linq;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class TechNode
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ResearchState State { get; private set; }
        public int ResearchTime { get; private set; }
        public int ResearchProgress { get; private set; }
        public string UnlockFeature { get; private set; }
        public List<TechNode> Prerequisites { get; private set; }

        private Dictionary<ResourceType, int> _costs;

        public TechNode(string name, string description, int researchTime = 0)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            State = ResearchState.Locked;
            ResearchTime = researchTime;
            ResearchProgress = 0;
            Prerequisites = new List<TechNode>();
            _costs = new Dictionary<ResourceType, int>();
        }

        public void SetCost(ResourceType resourceType, int amount)
        {
            _costs[resourceType] = amount;
        }

        public int GetCost(ResourceType resourceType)
        {
            return _costs.ContainsKey(resourceType) ? _costs[resourceType] : 0;
        }

        public void AddPrerequisite(TechNode prerequisite)
        {
            Prerequisites.Add(prerequisite);
        }

        public bool IsAvailable()
        {
            // Check if all prerequisites are completed
            foreach (var prereq in Prerequisites)
            {
                if (prereq.State != ResearchState.Completed)
                {
                    return false;
                }
            }
            return true;
        }

        public void StartResearch(ResourceInventory inventory)
        {
            // Validate state
            if (State == ResearchState.Completed)
            {
                throw new InvalidOperationException("Research already completed");
            }

            if (State == ResearchState.InProgress)
            {
                throw new InvalidOperationException("Research already in progress");
            }

            // Check if available
            if (!IsAvailable())
            {
                throw new InvalidOperationException("Research prerequisites not met");
            }

            // Check and consume resources
            foreach (var cost in _costs)
            {
                int available = inventory.GetResourceAmount(cost.Key);
                if (available < cost.Value)
                {
                    throw new InvalidOperationException($"Not enough {cost.Key}. Required: {cost.Value}, Available: {available}");
                }
            }

            // Consume all required resources
            foreach (var cost in _costs)
            {
                inventory.ConsumeResource(cost.Key, cost.Value);
            }

            // Start research
            State = ResearchState.InProgress;
            ResearchProgress = 0;
        }

        public void UpdateResearch(int deltaTime)
        {
            if (State != ResearchState.InProgress)
            {
                return;
            }

            ResearchProgress += deltaTime;

            // Check if research is complete
            if (ResearchProgress >= ResearchTime)
            {
                CompleteResearch();
            }
        }

        public void CompleteResearch()
        {
            State = ResearchState.Completed;
            ResearchProgress = ResearchTime;
        }

        public void SetUnlockFeature(string featureName)
        {
            UnlockFeature = featureName;
        }

        public bool IsFeatureUnlocked()
        {
            return State == ResearchState.Completed && !string.IsNullOrEmpty(UnlockFeature);
        }
    }
}
