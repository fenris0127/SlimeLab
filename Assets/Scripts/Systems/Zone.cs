using System;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class Zone
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public int Difficulty { get; private set; }
        public GridMap GridMap { get; private set; }
        public ZoneRequirement Requirement { get; private set; }

        public Zone(string name, int difficulty)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Difficulty = difficulty;
        }

        public void SetGridMap(GridMap gridMap)
        {
            GridMap = gridMap;
        }

        public void SetRequirement(ZoneRequirement requirement)
        {
            Requirement = requirement;
        }

        public bool CanEnter(Slime slime)
        {
            // If no requirement is set, anyone can enter
            if (Requirement == null)
            {
                return true;
            }

            return Requirement.IsMet(slime);
        }
    }
}
