namespace SlimeLab.Core
{
    public class EvolutionItem
    {
        public string Name { get; private set; }
        public ElementType Element { get; private set; }

        public EvolutionItem(string name, ElementType element)
        {
            Name = name;
            Element = element;
        }
    }
}
