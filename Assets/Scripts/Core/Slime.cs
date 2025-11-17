using System;

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

        public Slime(string name = "Unnamed Slime", ElementType element = ElementType.Neutral)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Element = element;
            Level = 1;
            Experience = 0;
            Stats = InitializeStats(element);
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
    }
}
