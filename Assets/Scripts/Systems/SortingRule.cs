using System.Collections.Generic;
using System.Linq;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class SortingRule
    {
        public ElementType? ElementFilter { get; private set; }
        public int? MinLevel { get; private set; }
        public int? MaxLevel { get; private set; }
        public SlimeMood? MoodFilter { get; private set; }

        public SortingRule(
            ElementType? elementFilter = null,
            int? minLevel = null,
            int? maxLevel = null,
            SlimeMood? moodFilter = null)
        {
            ElementFilter = elementFilter;
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            MoodFilter = moodFilter;
        }

        public List<Slime> Apply(List<Slime> slimes)
        {
            var filtered = slimes.AsEnumerable();

            // Apply element filter
            if (ElementFilter.HasValue)
            {
                filtered = filtered.Where(s => s.Element == ElementFilter.Value);
            }

            // Apply min level filter
            if (MinLevel.HasValue)
            {
                filtered = filtered.Where(s => s.Level >= MinLevel.Value);
            }

            // Apply max level filter
            if (MaxLevel.HasValue)
            {
                filtered = filtered.Where(s => s.Level <= MaxLevel.Value);
            }

            // Apply mood filter
            if (MoodFilter.HasValue)
            {
                filtered = filtered.Where(s => s.Mood == MoodFilter.Value);
            }

            return filtered.ToList();
        }
    }
}
