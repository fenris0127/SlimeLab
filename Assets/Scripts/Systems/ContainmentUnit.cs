using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class ContainmentUnit
    {
        public Slime AssignedSlime { get; private set; }
        public bool HasSlime => AssignedSlime != null;
        public EnvironmentType EnvironmentType { get; private set; }

        public ContainmentUnit(EnvironmentType environmentType = EnvironmentType.Standard)
        {
            EnvironmentType = environmentType;
        }

        public void AssignSlime(Slime slime)
        {
            AssignedSlime = slime;
        }

        public Slime RemoveSlime()
        {
            var slime = AssignedSlime;
            AssignedSlime = null;
            return slime;
        }

        public float GetEfficiency()
        {
            // Empty unit is always 100% efficient
            if (!HasSlime)
            {
                return 1.0f;
            }

            // Standard environment gives 90% efficiency to all slimes
            if (EnvironmentType == EnvironmentType.Standard)
            {
                return 0.9f;
            }

            // Neutral slimes get 85% efficiency in any specialized environment
            if (AssignedSlime.Element == ElementType.Neutral)
            {
                return 0.85f;
            }

            // Check if element matches environment
            bool isMatched = IsElementMatchedToEnvironment(AssignedSlime.Element, EnvironmentType);
            return isMatched ? 1.0f : 0.7f;
        }

        private bool IsElementMatchedToEnvironment(ElementType element, EnvironmentType environment)
        {
            switch (environment)
            {
                case EnvironmentType.Volcanic:
                    return element == ElementType.Fire;
                case EnvironmentType.Aquatic:
                    return element == ElementType.Water;
                case EnvironmentType.Storm:
                    return element == ElementType.Electric;
                default:
                    return false;
            }
        }
    }
}
