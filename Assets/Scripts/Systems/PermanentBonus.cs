namespace SlimeLab.Systems
{
    public class PermanentBonus
    {
        public BonusType Type { get; private set; }
        public float Value { get; private set; }
        public string Description { get; private set; }
        public bool IsPercentage { get; private set; }

        public PermanentBonus(BonusType type, float value, string description = "", bool isPercentage = true)
        {
            Type = type;
            Value = value;
            Description = description;
            IsPercentage = isPercentage;
        }
    }
}
