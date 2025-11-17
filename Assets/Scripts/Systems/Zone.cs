using System;
using System.Collections.Generic;
using System.Linq;
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
        public int ResourceNodeCount => _resourceNodes.Count;

        private List<ResourceNode> _resourceNodes;

        public Zone(string name, int difficulty)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Difficulty = difficulty;
            _resourceNodes = new List<ResourceNode>();
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

        public void AddResourceNode(ResourceNode node)
        {
            _resourceNodes.Add(node);
        }

        public ResourceNode GetResourceNodeAt(int x, int y)
        {
            return _resourceNodes.FirstOrDefault(node => node.X == x && node.Y == y);
        }

        public List<ResourceNode> GetAllResourceNodes()
        {
            return new List<ResourceNode>(_resourceNodes);
        }
    }
}
