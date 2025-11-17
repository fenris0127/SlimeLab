using System;
using System.Collections.Generic;

namespace SlimeLab.Core
{
    public class Slime
    {
        // Base stat constants
        private const int BASE_HP = 50;
        private const int BASE_ATTACK = 10;
        private const int BASE_DEFENSE = 5;
        private const int BASE_SPEED = 8;

        // Element modifier constants
        private const int FIRE_ATTACK_BONUS = 5;
        private const int FIRE_SPEED_BONUS = 2;
        private const int WATER_HP_BONUS = 20;
        private const int WATER_DEFENSE_BONUS = 3;
        private const int ELECTRIC_HP_PENALTY = 10;
        private const int ELECTRIC_ATTACK_BONUS = 3;
        private const int ELECTRIC_SPEED_BONUS = 5;

        // Hunger and mood thresholds
        private const int HUNGER_MAX = 100;
        private const int HUNGER_UNHAPPY_THRESHOLD = 91;
        private const int HUNGER_SAD_THRESHOLD = 61;

        // Evolution constants
        private const int MIN_EVOLUTION_LEVEL = 10;
        private const int DEFAULT_EVOLUTION_HP_BOOST = 20;
        private const int DEFAULT_EVOLUTION_ATTACK_BOOST = 10;
        private const int DEFAULT_EVOLUTION_DEFENSE_BOOST = 5;
        private const int DEFAULT_EVOLUTION_SPEED_BOOST = 5;
        private const int MIN_AFFINITY_FOR_EVOLUTION = 70;

        // Experience and affinity constants
        private const int AFFINITY_MAX = 100;
        private const int FEEDING_EXPERIENCE_DIVISOR = 2;

        public string ID { get; private set; }
        public string Name { get; private set; }
        public ElementType Element { get; private set; }
        public int Level { get; private set; }
        public int Experience { get; private set; }
        public SlimeStats Stats { get; private set; }
        public int Hunger { get; private set; }
        public SlimeMood Mood { get; private set; }
        public List<Gene> Genes { get; private set; }
        public int Affinity { get; private set; }

        public Slime(string name = "Unnamed Slime", ElementType element = ElementType.Neutral)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Element = element;
            Level = 1;
            Experience = 0;
            Stats = InitializeStats(element);
            Hunger = 0;
            Mood = SlimeMood.Happy;
            Genes = new List<Gene>();
            Affinity = 0;
        }

        private SlimeStats InitializeStats(ElementType element)
        {
            // Element-specific modifiers
            switch (element)
            {
                case ElementType.Fire:
                    return new SlimeStats(BASE_HP, BASE_ATTACK + FIRE_ATTACK_BONUS, BASE_DEFENSE, BASE_SPEED + FIRE_SPEED_BONUS);
                case ElementType.Water:
                    return new SlimeStats(BASE_HP + WATER_HP_BONUS, BASE_ATTACK, BASE_DEFENSE + WATER_DEFENSE_BONUS, BASE_SPEED);
                case ElementType.Electric:
                    return new SlimeStats(BASE_HP - ELECTRIC_HP_PENALTY, BASE_ATTACK + ELECTRIC_ATTACK_BONUS, BASE_DEFENSE, BASE_SPEED + ELECTRIC_SPEED_BONUS);
                case ElementType.Neutral:
                default:
                    return new SlimeStats(BASE_HP, BASE_ATTACK, BASE_DEFENSE, BASE_SPEED);
            }
        }

        public void IncreaseHunger(int amount)
        {
            Hunger = Math.Min(Hunger + amount, HUNGER_MAX);
            UpdateMood();
        }

        public void Feed(int amount)
        {
            Hunger = Math.Max(Hunger - amount, 0);

            // Gain experience from feeding
            Experience += amount / FEEDING_EXPERIENCE_DIVISOR;

            UpdateMood();
        }

        private void UpdateMood()
        {
            if (Hunger >= HUNGER_UNHAPPY_THRESHOLD)
            {
                Mood = SlimeMood.Unhappy;
            }
            else if (Hunger >= HUNGER_SAD_THRESHOLD)
            {
                Mood = SlimeMood.Sad;
            }
            else
            {
                Mood = SlimeMood.Happy;
            }
        }

        public void AddGene(Gene gene)
        {
            if (gene != null && !HasGene(gene.ID))
            {
                Genes.Add(gene);
            }
        }

        public Gene GetGene(string geneID)
        {
            return Genes.Find(g => g.ID == geneID);
        }

        public bool HasGene(string geneID)
        {
            return Genes.Exists(g => g.ID == geneID);
        }

        public void SetLevel(int level)
        {
            Level = level;
        }

        public bool CanEvolve()
        {
            return Level >= MIN_EVOLUTION_LEVEL;
        }

        public void Evolve(EvolutionItem item)
        {
            if (!CanEvolve())
            {
                throw new InvalidOperationException($"Slime must be at least level {MIN_EVOLUTION_LEVEL} to evolve. Current level: {Level}");
            }

            // Check if evolution item matches slime element (optional, but recommended)
            if (item.Element != Element)
            {
                // Allow evolution but maybe with reduced benefits in the future
            }

            // Use a simple evolution tree for determining evolution
            var evolutionTree = new Systems.EvolutionTree();
            var evolutionPath = evolutionTree.GetEvolutionPath(this);

            if (evolutionPath != null)
            {
                // Apply evolution
                Name = evolutionPath.TargetName;
                Level += 1;
                Stats.BoostStats(
                    evolutionPath.HPBoost,
                    evolutionPath.AttackBoost,
                    evolutionPath.DefenseBoost,
                    evolutionPath.SpeedBoost
                );
            }
            else
            {
                // Default evolution if no path found
                Name = $"Evolved {Name}";
                Level += 1;
                Stats.BoostStats(DEFAULT_EVOLUTION_HP_BOOST, DEFAULT_EVOLUTION_ATTACK_BOOST, DEFAULT_EVOLUTION_DEFENSE_BOOST, DEFAULT_EVOLUTION_SPEED_BOOST);
            }
        }

        // Affinity management
        public void SetAffinity(int affinity)
        {
            Affinity = Math.Clamp(affinity, 0, AFFINITY_MAX);
        }

        public void IncreaseAffinity(int amount)
        {
            Affinity = Math.Min(Affinity + amount, AFFINITY_MAX);
        }

        // Environment-based evolution
        public bool CanEvolveInEnvironment(Systems.EnvironmentType environment, EvolutionItem item)
        {
            if (!CanEvolve()) return false;

            // Check if element matches environment
            return IsElementMatchedToEnvironment(Element, environment);
        }

        public void EvolveInEnvironment(Systems.EnvironmentType environment, EvolutionItem item)
        {
            if (!CanEvolveInEnvironment(environment, item))
            {
                throw new InvalidOperationException("Cannot evolve in this environment");
            }

            var registry = new Systems.SpecialEvolutionRegistry();
            var specialEvolution = registry.GetEnvironmentEvolution(Element, environment);
            ApplySpecialEvolution(specialEvolution);
        }

        private void ApplySpecialEvolution(Systems.SpecialEvolutionData evolution)
        {
            if (evolution == null) return;

            Name = evolution.TargetName;
            Level += 1;
            Stats.BoostStats(
                evolution.HPBoost,
                evolution.AttackBoost,
                evolution.DefenseBoost,
                evolution.SpeedBoost
            );
        }

        private bool IsElementMatchedToEnvironment(ElementType element, Systems.EnvironmentType environment)
        {
            switch (environment)
            {
                case Systems.EnvironmentType.Volcanic:
                    return element == ElementType.Fire;
                case Systems.EnvironmentType.Aquatic:
                    return element == ElementType.Water;
                case Systems.EnvironmentType.Storm:
                    return element == ElementType.Electric;
                default:
                    return false;
            }
        }

        // Time-based evolution
        public bool CanEvolveAtTime(EvolutionItem item, DateTime currentTime)
        {
            if (!CanEvolve()) return false;

            var registry = new Systems.SpecialEvolutionRegistry();
            return registry.CanEvolveAtTime(item, currentTime);
        }

        public void EvolveAtTime(EvolutionItem item, DateTime currentTime)
        {
            if (!CanEvolveAtTime(item, currentTime))
            {
                throw new InvalidOperationException("Cannot evolve at this time");
            }

            var registry = new Systems.SpecialEvolutionRegistry();
            var timeEvolution = registry.GetTimeBasedEvolution(item, currentTime);
            ApplySpecialEvolution(timeEvolution);
        }

        // Affinity-based evolution
        public bool CanEvolveWithAffinity(EvolutionItem item)
        {
            if (!CanEvolve()) return false;

            // Require high affinity
            return Affinity >= MIN_AFFINITY_FOR_EVOLUTION;
        }

        public void EvolveWithAffinity(EvolutionItem item)
        {
            if (!CanEvolveWithAffinity(item))
            {
                throw new InvalidOperationException($"Affinity too low for evolution. Current: {Affinity}, Required: {MIN_AFFINITY_FOR_EVOLUTION}");
            }

            var registry = new Systems.SpecialEvolutionRegistry();
            var affinityEvolution = registry.GetAffinityEvolution(item);
            ApplySpecialEvolution(affinityEvolution);
        }

        // Resource gathering
        public Resource GatherResource(Systems.ResourceNode node)
        {
            // Cannot gather from depleted node
            if (node.IsDepleted())
            {
                return null;
            }

            // Base gathering amount based on level (1-2 per level)
            int baseAmount = Level + (Level / 2);

            // Apply environmental bonus if applicable
            float bonus = node.GetGatheringBonus(Element);
            int gatheredAmount = (int)(baseAmount * bonus);

            // Ensure we don't gather more than available
            if (gatheredAmount > node.Amount)
            {
                gatheredAmount = node.Amount;
            }

            // Deplete the node
            node.Deplete(gatheredAmount);

            // Return the gathered resource
            return new Resource(node.ResourceType, gatheredAmount);
        }
    }
}
