using System;

namespace SlimeLab.Core
{
    public class Gene
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public GeneType Type { get; private set; }

        public Gene(string name, GeneType type = GeneType.Recessive)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Type = type;
        }
    }
}
