using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class ResourceNode
    {
        public ResourceType ResourceType { get; private set; }
        public int Amount { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public ElementType? EnvironmentBonus { get; private set; }

        public ResourceNode(ResourceType resourceType, int amount, int x = 0, int y = 0)
        {
            ResourceType = resourceType;
            Amount = amount;
            X = x;
            Y = y;
            EnvironmentBonus = null;
        }

        public void SetEnvironmentBonus(ElementType elementType)
        {
            EnvironmentBonus = elementType;
        }

        public void Deplete(int amount)
        {
            Amount -= amount;
            if (Amount < 0)
            {
                Amount = 0;
            }
        }

        public bool IsDepleted()
        {
            return Amount <= 0;
        }

        public float GetGatheringBonus(ElementType gathererElement)
        {
            // If node has environmental bonus and gatherer element matches
            if (EnvironmentBonus.HasValue && EnvironmentBonus.Value == gathererElement)
            {
                return 1.5f; // 50% bonus
            }

            return 1.0f;
        }
    }
}
