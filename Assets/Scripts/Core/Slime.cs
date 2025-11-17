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

        public Slime(string name = "Unnamed Slime", ElementType element = ElementType.Neutral)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Element = element;
            Level = 1;
            Experience = 0;
        }
    }
}
