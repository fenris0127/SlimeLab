using SlimeLab.Core;

namespace SlimeLab.UI
{
    public class ResourceBarData
    {
        public ResourceType ResourceType { get; private set; }
        public int CurrentAmount { get; private set; }
        public int MaxAmount { get; private set; }
        public float Percentage { get; private set; }

        public ResourceBarData(ResourceType resourceType, int currentAmount, int maxAmount)
        {
            ResourceType = resourceType;
            CurrentAmount = currentAmount;
            MaxAmount = maxAmount;
            CalculatePercentage();
        }

        public void Update(int currentAmount, int maxAmount)
        {
            CurrentAmount = currentAmount;
            MaxAmount = maxAmount;
            CalculatePercentage();
        }

        public void SetMaxAmount(int maxAmount)
        {
            MaxAmount = maxAmount;
            CalculatePercentage();
        }

        private void CalculatePercentage()
        {
            if (MaxAmount > 0)
            {
                Percentage = (float)CurrentAmount / MaxAmount;
            }
            else
            {
                Percentage = 0f;
            }
        }
    }
}
