using System;
using System.Collections.Generic;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class SpecialEvolutionData
    {
        public string TargetName { get; set; }
        public int HPBoost { get; set; }
        public int AttackBoost { get; set; }
        public int DefenseBoost { get; set; }
        public int SpeedBoost { get; set; }

        public SpecialEvolutionData(string targetName, int hpBoost, int attackBoost, int defenseBoost, int speedBoost)
        {
            TargetName = targetName;
            HPBoost = hpBoost;
            AttackBoost = attackBoost;
            DefenseBoost = defenseBoost;
            SpeedBoost = speedBoost;
        }
    }

    public class SpecialEvolutionRegistry
    {
        // Environment-based evolutions
        private Dictionary<(ElementType, EnvironmentType), SpecialEvolutionData> _environmentEvolutions;

        // Time-based evolutions
        private Dictionary<string, (SpecialEvolutionData data, Func<DateTime, bool> timeCondition)> _timeEvolutions;

        // Affinity-based evolutions
        private Dictionary<string, SpecialEvolutionData> _affinityEvolutions;

        public SpecialEvolutionRegistry()
        {
            InitializeEnvironmentEvolutions();
            InitializeTimeBasedEvolutions();
            InitializeAffinityEvolutions();
        }

        private void InitializeEnvironmentEvolutions()
        {
            _environmentEvolutions = new Dictionary<(ElementType, EnvironmentType), SpecialEvolutionData>();

            // Fire slime in Volcanic environment
            _environmentEvolutions.Add(
                (ElementType.Fire, EnvironmentType.Volcanic),
                new SpecialEvolutionData("Magma Titan", 60, 25, 20, 15)
            );

            // Water slime in Aquatic environment
            _environmentEvolutions.Add(
                (ElementType.Water, EnvironmentType.Aquatic),
                new SpecialEvolutionData("Oceanic Guardian", 70, 20, 25, 12)
            );

            // Electric slime in Storm environment
            _environmentEvolutions.Add(
                (ElementType.Electric, EnvironmentType.Storm),
                new SpecialEvolutionData("Tempest Sovereign", 55, 30, 15, 25)
            );
        }

        private void InitializeTimeBasedEvolutions()
        {
            _timeEvolutions = new Dictionary<string, (SpecialEvolutionData, Func<DateTime, bool>)>();

            // Moon Stone - evolves at night (20:00 - 6:00)
            _timeEvolutions.Add(
                "Moon Stone",
                (
                    new SpecialEvolutionData("Lunar Eclipse", 65, 22, 18, 20),
                    (DateTime time) => time.Hour >= 20 || time.Hour < 6
                )
            );

            // Sun Stone - evolves during day (6:00 - 20:00)
            _timeEvolutions.Add(
                "Sun Stone",
                (
                    new SpecialEvolutionData("Solar Flare", 60, 28, 16, 18),
                    (DateTime time) => time.Hour >= 6 && time.Hour < 20
                )
            );
        }

        private void InitializeAffinityEvolutions()
        {
            _affinityEvolutions = new Dictionary<string, SpecialEvolutionData>();

            // Friendship Stone - requires high affinity
            _affinityEvolutions.Add(
                "Friendship Stone",
                new SpecialEvolutionData("Eternal Bond", 80, 35, 30, 25)
            );
        }

        // Environment evolution methods
        public SpecialEvolutionData GetEnvironmentEvolution(ElementType element, EnvironmentType environment)
        {
            if (_environmentEvolutions.TryGetValue((element, environment), out var evolution))
            {
                return evolution;
            }
            return null;
        }

        // Time-based evolution methods
        public bool CanEvolveAtTime(EvolutionItem item, DateTime currentTime)
        {
            if (_timeEvolutions.TryGetValue(item.Name, out var evolutionInfo))
            {
                return evolutionInfo.timeCondition(currentTime);
            }
            return false;
        }

        public SpecialEvolutionData GetTimeBasedEvolution(EvolutionItem item, DateTime currentTime)
        {
            if (_timeEvolutions.TryGetValue(item.Name, out var evolutionInfo))
            {
                if (evolutionInfo.timeCondition(currentTime))
                {
                    return evolutionInfo.data;
                }
            }
            return null;
        }

        // Affinity-based evolution methods
        public SpecialEvolutionData GetAffinityEvolution(EvolutionItem item)
        {
            if (_affinityEvolutions.TryGetValue(item.Name, out var evolution))
            {
                return evolution;
            }
            return null;
        }
    }
}
