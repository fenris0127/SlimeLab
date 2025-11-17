using System;
using System.Collections.Generic;

namespace SlimeLab.Core
{
    public class Slime
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public ElementType Element { get; private set; }
        public int Level { get; private set; }
        public int Experience { get; private set; }
        public SlimeStats Stats { get; private set; }
        public int Hunger { get; private set; }
        public SlimeMood Mood { get; private set; }
        public List<Gene> Genes { get; private set; }

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
        }

        private SlimeStats InitializeStats(ElementType element)
        {
            // Base stats for level 1 slime
            int baseHP = 50;
            int baseAttack = 10;
            int baseDefense = 5;
            int baseSpeed = 8;

            // Element-specific modifiers
            switch (element)
            {
                case ElementType.Fire:
                    return new SlimeStats(baseHP, baseAttack + 5, baseDefense, baseSpeed + 2);
                case ElementType.Water:
                    return new SlimeStats(baseHP + 20, baseAttack, baseDefense + 3, baseSpeed);
                case ElementType.Electric:
                    return new SlimeStats(baseHP - 10, baseAttack + 3, baseDefense, baseSpeed + 5);
                case ElementType.Neutral:
                default:
                    return new SlimeStats(baseHP, baseAttack, baseDefense, baseSpeed);
            }
        }

        public void IncreaseHunger(int amount)
        {
            Hunger = Math.Min(Hunger + amount, 100);
            UpdateMood();
        }

        public void Feed(int amount)
        {
            Hunger = Math.Max(Hunger - amount, 0);

            // Gain experience from feeding
            Experience += amount / 2;

            UpdateMood();
        }

        private void UpdateMood()
        {
            if (Hunger >= 91)
            {
                Mood = SlimeMood.Unhappy;
            }
            else if (Hunger >= 61)
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
    }
}
