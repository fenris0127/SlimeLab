using System;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class AutoFeeder
    {
        public int FeedInterval { get; private set; }
        public int FeedAmount { get; private set; }
        public int TimeSinceLastFeed { get; private set; }
        public bool IsActive { get; private set; }

        private Slime _attachedSlime;
        private ResourceInventory _resourceInventory;

        public AutoFeeder(int feedInterval, int feedAmount = 20)
        {
            FeedInterval = feedInterval;
            FeedAmount = feedAmount;
            TimeSinceLastFeed = 0;
            IsActive = true;
        }

        public void AttachSlime(Slime slime)
        {
            _attachedSlime = slime;
        }

        public void DetachSlime()
        {
            _attachedSlime = null;
        }

        public void SetResourceInventory(ResourceInventory inventory)
        {
            _resourceInventory = inventory;
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        public void Update(int deltaTime)
        {
            // Don't update if inactive
            if (!IsActive)
            {
                return;
            }

            // Don't update if no slime attached
            if (_attachedSlime == null)
            {
                return;
            }

            TimeSinceLastFeed += deltaTime;

            // Check if it's time to feed
            if (TimeSinceLastFeed >= FeedInterval)
            {
                TryFeed();
                TimeSinceLastFeed = 0;
            }
        }

        private void TryFeed()
        {
            // Calculate resource cost (feed amount / 2, rounded up)
            int resourceCost = (FeedAmount + 1) / 2;

            // Check if we have enough resources
            if (_resourceInventory != null)
            {
                int availableFood = _resourceInventory.GetResourceAmount(ResourceType.Food);

                if (availableFood < resourceCost)
                {
                    // Not enough resources, don't feed
                    return;
                }

                // Consume resources
                _resourceInventory.ConsumeResource(ResourceType.Food, resourceCost);
            }

            // Feed the slime
            if (_attachedSlime != null)
            {
                _attachedSlime.Feed(FeedAmount);
            }
        }
    }
}
