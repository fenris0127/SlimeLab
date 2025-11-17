using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class ContainmentUnit
    {
        public Slime AssignedSlime { get; private set; }
        public bool HasSlime => AssignedSlime != null;
        public EnvironmentType EnvironmentType { get; private set; }
        public AutoFeeder Feeder { get; private set; }
        public ResourceCollector Collector { get; private set; }

        public ContainmentUnit(EnvironmentType environmentType = EnvironmentType.Standard)
        {
            EnvironmentType = environmentType;
        }

        public void AssignSlime(Slime slime)
        {
            AssignedSlime = slime;

            // If feeder is attached, attach the slime to it
            if (Feeder != null)
            {
                Feeder.AttachSlime(slime);
            }

            // If collector is attached, attach the slime to it
            if (Collector != null)
            {
                Collector.AttachSlime(slime);
            }
        }

        public Slime RemoveSlime()
        {
            var slime = AssignedSlime;
            AssignedSlime = null;

            // Detach slime from feeder if attached
            if (Feeder != null)
            {
                Feeder.DetachSlime();
            }

            // Detach slime from collector if attached
            if (Collector != null)
            {
                Collector.DetachSlime();
            }

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

        public void AttachFeeder(AutoFeeder feeder)
        {
            Feeder = feeder;

            // If there's already a slime assigned, attach it to the feeder
            if (HasSlime)
            {
                Feeder.AttachSlime(AssignedSlime);
            }
        }

        public void DetachFeeder()
        {
            if (Feeder != null)
            {
                Feeder.DetachSlime();
                Feeder = null;
            }
        }

        public void AttachCollector(ResourceCollector collector)
        {
            Collector = collector;

            // If there's already a slime assigned, attach it to the collector
            if (HasSlime)
            {
                Collector.AttachSlime(AssignedSlime);
            }
        }

        public void DetachCollector()
        {
            if (Collector != null)
            {
                Collector.DetachSlime();
                Collector = null;
            }
        }

        public void Update(int deltaTime)
        {
            // Update feeder if attached
            if (Feeder != null)
            {
                Feeder.Update(deltaTime);
            }

            // Update collector if attached
            if (Collector != null)
            {
                Collector.Update(deltaTime);
            }
        }
    }
}
