using System;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class ResourceCollector
    {
        public int CollectionInterval { get; private set; }
        public int TimeSinceLastCollection { get; private set; }
        public float Efficiency { get; private set; }
        public int Level { get; private set; }
        public bool IsActive { get; private set; }

        private const int MAX_LEVEL = 10;
        private Slime _attachedSlime;
        private ResourceInventory _resourceInventory;

        public ResourceCollector(int collectionInterval)
        {
            CollectionInterval = collectionInterval;
            TimeSinceLastCollection = 0;
            Level = 1;
            Efficiency = 1.0f;
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

        public void Upgrade()
        {
            if (Level >= MAX_LEVEL)
            {
                return; // Cannot upgrade beyond max level
            }

            Level++;

            // Efficiency increases by 15% per level
            Efficiency = 1.0f + ((Level - 1) * 0.15f);
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

            TimeSinceLastCollection += deltaTime;

            // Check if it's time to collect
            if (TimeSinceLastCollection >= CollectionInterval)
            {
                CollectResources();
                TimeSinceLastCollection = 0;
            }
        }

        private void CollectResources()
        {
            if (_attachedSlime == null || _resourceInventory == null)
            {
                return;
            }

            // Base collection amount based on slime level
            int baseAmount = _attachedSlime.Level;

            // Apply efficiency multiplier
            int collectionAmount = (int)(baseAmount * Efficiency);

            // Minimum 1 resource collected
            if (collectionAmount < 1)
            {
                collectionAmount = 1;
            }

            // Determine resource type based on slime element
            ResourceType primaryResource = GetPrimaryResourceType(_attachedSlime.Element);

            // Collect resources
            _resourceInventory.AddResource(new Resource(primaryResource, collectionAmount));

            // Small chance to collect secondary resource
            if (_attachedSlime.Level > 5)
            {
                ResourceType secondaryResource = GetSecondaryResourceType(_attachedSlime.Element);
                int secondaryAmount = collectionAmount / 3;
                if (secondaryAmount > 0)
                {
                    _resourceInventory.AddResource(new Resource(secondaryResource, secondaryAmount));
                }
            }
        }

        private ResourceType GetPrimaryResourceType(ElementType element)
        {
            switch (element)
            {
                case ElementType.Fire:
                    return ResourceType.Energy;
                case ElementType.Water:
                    return ResourceType.Material;
                case ElementType.Electric:
                    return ResourceType.Energy;
                case ElementType.Neutral:
                default:
                    return ResourceType.Material;
            }
        }

        private ResourceType GetSecondaryResourceType(ElementType element)
        {
            switch (element)
            {
                case ElementType.Fire:
                    return ResourceType.Material;
                case ElementType.Water:
                    return ResourceType.Food;
                case ElementType.Electric:
                    return ResourceType.Research;
                case ElementType.Neutral:
                default:
                    return ResourceType.Energy;
            }
        }
    }
}
