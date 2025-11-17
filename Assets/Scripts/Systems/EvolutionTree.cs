using System.Collections.Generic;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class EvolutionPath
    {
        public string TargetName { get; set; }
        public int RequiredLevel { get; set; }
        public ElementType Element { get; set; }
        public int HPBoost { get; set; }
        public int AttackBoost { get; set; }
        public int DefenseBoost { get; set; }
        public int SpeedBoost { get; set; }

        public EvolutionPath(string targetName, int requiredLevel, ElementType element,
            int hpBoost, int attackBoost, int defenseBoost, int speedBoost)
        {
            TargetName = targetName;
            RequiredLevel = requiredLevel;
            Element = element;
            HPBoost = hpBoost;
            AttackBoost = attackBoost;
            DefenseBoost = defenseBoost;
            SpeedBoost = speedBoost;
        }
    }

    public class EvolutionTree
    {
        private Dictionary<ElementType, List<EvolutionPath>> _evolutionPaths;

        public EvolutionTree()
        {
            _evolutionPaths = new Dictionary<ElementType, List<EvolutionPath>>();
            InitializeEvolutionPaths();
        }

        private void InitializeEvolutionPaths()
        {
            // Fire evolution paths
            _evolutionPaths[ElementType.Fire] = new List<EvolutionPath>
            {
                new EvolutionPath("Inferno Slime", 10, ElementType.Fire, 30, 15, 5, 5),
                new EvolutionPath("Flame Lord", 20, ElementType.Fire, 60, 30, 10, 10)
            };

            // Water evolution paths
            _evolutionPaths[ElementType.Water] = new List<EvolutionPath>
            {
                new EvolutionPath("Aqua Slime", 10, ElementType.Water, 40, 10, 10, 5),
                new EvolutionPath("Ocean King", 20, ElementType.Water, 80, 20, 20, 10)
            };

            // Electric evolution paths
            _evolutionPaths[ElementType.Electric] = new List<EvolutionPath>
            {
                new EvolutionPath("Thunder Slime", 10, ElementType.Electric, 25, 12, 5, 15),
                new EvolutionPath("Storm Emperor", 20, ElementType.Electric, 50, 24, 10, 30)
            };

            // Neutral evolution paths
            _evolutionPaths[ElementType.Neutral] = new List<EvolutionPath>
            {
                new EvolutionPath("Balanced Slime", 10, ElementType.Neutral, 30, 12, 8, 8),
                new EvolutionPath("Harmonious Master", 20, ElementType.Neutral, 60, 24, 16, 16)
            };
        }

        public EvolutionPath GetEvolutionPath(Slime slime)
        {
            if (!_evolutionPaths.ContainsKey(slime.Element))
            {
                return null;
            }

            var paths = _evolutionPaths[slime.Element];

            // Find the appropriate evolution for the slime's level
            EvolutionPath bestPath = null;
            foreach (var path in paths)
            {
                if (slime.Level >= path.RequiredLevel)
                {
                    if (bestPath == null || path.RequiredLevel > bestPath.RequiredLevel)
                    {
                        bestPath = path;
                    }
                }
            }

            // If no exact match, return the first available path
            return bestPath ?? (paths.Count > 0 ? paths[0] : null);
        }

        public List<EvolutionPath> GetAllPathsForElement(ElementType element)
        {
            if (_evolutionPaths.ContainsKey(element))
            {
                return new List<EvolutionPath>(_evolutionPaths[element]);
            }
            return new List<EvolutionPath>();
        }
    }
}
