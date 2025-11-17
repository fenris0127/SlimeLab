using SlimeLab.Core;
using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.UI
{
    public class ResourceBarCollection
    {
        private Dictionary<ResourceType, ResourceBarData> _bars;

        public ResourceBarCollection(ResourceInventory inventory)
        {
            _bars = new Dictionary<ResourceType, ResourceBarData>();
            Refresh(inventory);
        }

        public void Refresh(ResourceInventory inventory)
        {
            _bars.Clear();

            var resources = inventory.GetAllResources();
            foreach (var resource in resources)
            {
                // Use a large max amount for display (can be customized)
                int maxAmount = 1000;
                _bars[resource.Type] = new ResourceBarData(resource.Type, resource.Amount, maxAmount);
            }
        }

        public ResourceBarData GetBar(ResourceType resourceType)
        {
            return _bars.ContainsKey(resourceType) ? _bars[resourceType] : null;
        }

        public List<ResourceBarData> GetBars()
        {
            return new List<ResourceBarData>(_bars.Values);
        }
    }
}
