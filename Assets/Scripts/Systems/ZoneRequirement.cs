using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class ZoneRequirement
    {
        public int MinLevel { get; private set; }
        public ElementType? RequiredElement { get; private set; }

        public ZoneRequirement(int minLevel = 1, ElementType? requiredElement = null)
        {
            MinLevel = minLevel;
            RequiredElement = requiredElement;
        }

        public bool IsMet(Slime slime)
        {
            // Check level requirement
            if (slime.Level < MinLevel)
            {
                return false;
            }

            // Check element requirement if specified
            if (RequiredElement.HasValue && slime.Element != RequiredElement.Value)
            {
                return false;
            }

            return true;
        }
    }
}
