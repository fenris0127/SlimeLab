using System.Collections.Generic;
using System.Linq;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class SlimeSorter
    {
        public Dictionary<ElementType, List<Slime>> SortByElement(List<Slime> slimes)
        {
            var sorted = new Dictionary<ElementType, List<Slime>>();

            foreach (var slime in slimes)
            {
                if (!sorted.ContainsKey(slime.Element))
                {
                    sorted[slime.Element] = new List<Slime>();
                }
                sorted[slime.Element].Add(slime);
            }

            return sorted;
        }

        public List<Slime> SortByLevel(List<Slime> slimes)
        {
            return slimes.OrderByDescending(s => s.Level).ToList();
        }

        public List<Slime> SortByName(List<Slime> slimes)
        {
            return slimes.OrderBy(s => s.Name).ToList();
        }

        public List<Slime> ApplyRule(List<Slime> slimes, SortingRule rule)
        {
            return rule.Apply(slimes);
        }

        public List<Slime> ApplyRules(List<Slime> slimes, List<SortingRule> rules)
        {
            var filtered = slimes;

            foreach (var rule in rules)
            {
                filtered = rule.Apply(filtered);
            }

            return filtered;
        }

        public List<Slime> GetSlimesByElement(Laboratory laboratory, ElementType elementType)
        {
            var allSlimes = laboratory.GetAllSlimes();
            return allSlimes.Where(s => s.Element == elementType).ToList();
        }

        public List<Slime> GetTopLevelSlimes(List<Slime> slimes, int count)
        {
            return slimes
                .OrderByDescending(s => s.Level)
                .Take(count)
                .ToList();
        }
    }
}
