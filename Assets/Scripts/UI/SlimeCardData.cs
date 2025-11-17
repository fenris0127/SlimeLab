using SlimeLab.Core;

namespace SlimeLab.UI
{
    public class SlimeCardData
    {
        public string Name { get; private set; }
        public ElementType Element { get; private set; }
        public int Level { get; private set; }
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Hunger { get; private set; }
        public SlimeMood Mood { get; private set; }
        public int Experience { get; private set; }
        public int ExperienceToNextLevel { get; private set; }

        public SlimeCardData(Slime slime)
        {
            Refresh(slime);
        }

        public void Refresh(Slime slime)
        {
            Name = slime.Name;
            Element = slime.Element;
            Level = slime.Level;
            HP = slime.Stats.HP;
            MaxHP = slime.Stats.MaxHP;
            Attack = slime.Stats.Attack;
            Defense = slime.Stats.Defense;
            Hunger = slime.Hunger;
            Mood = slime.Mood;
            Experience = slime.Experience;
            ExperienceToNextLevel = slime.ExperienceToNextLevel;
        }

        public string GetDisplayText()
        {
            return $"{Name} (Lv.{Level}) - {Element}";
        }
    }
}
