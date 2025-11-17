using System.Collections.Generic;

namespace SlimeLab.Core
{
    public class ResourceInventory
    {
        private Dictionary<ResourceType, int> _resources;

        public ResourceInventory()
        {
            _resources = new Dictionary<ResourceType, int>();
        }

        public void Add(Resource resource)
        {
            if (_resources.ContainsKey(resource.Type))
            {
                _resources[resource.Type] += resource.Amount;
            }
            else
            {
                _resources[resource.Type] = resource.Amount;
            }
        }

        public void Consume(ResourceType type, int amount)
        {
            int available = GetAmount(type);

            if (available < amount)
            {
                throw new InsufficientResourceException(type, amount, available);
            }

            _resources[type] -= amount;
        }

        public int GetAmount(ResourceType type)
        {
            return _resources.ContainsKey(type) ? _resources[type] : 0;
        }
    }
}
