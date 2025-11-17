using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class TechTree
    {
        public int NodeCount => _nodes.Count;

        private List<TechNode> _nodes;

        public TechTree()
        {
            _nodes = new List<TechNode>();
        }

        public void AddNode(TechNode node)
        {
            _nodes.Add(node);
        }

        public TechNode GetNode(string name)
        {
            return _nodes.FirstOrDefault(n => n.Name == name);
        }

        public IReadOnlyList<TechNode> GetAvailableNodes()
        {
            return _nodes
                .Where(n => n.IsAvailable() && n.State != ResearchState.Completed && n.State != ResearchState.InProgress)
                .ToList();
        }

        public IReadOnlyList<TechNode> GetCompletedNodes()
        {
            return _nodes
                .Where(n => n.State == ResearchState.Completed)
                .ToList();
        }

        public IReadOnlyList<TechNode> GetNodesInProgress()
        {
            return _nodes
                .Where(n => n.State == ResearchState.InProgress)
                .ToList();
        }

        public IReadOnlyList<string> GetUnlockedFeatures()
        {
            return _nodes
                .Where(n => n.IsFeatureUnlocked())
                .Select(n => n.UnlockFeature)
                .ToList();
        }

        public bool IsFeatureUnlocked(string featureName)
        {
            return _nodes.Any(n => n.IsFeatureUnlocked() && n.UnlockFeature == featureName);
        }

        public IReadOnlyList<TechNode> GetAllNodes()
        {
            return new List<TechNode>(_nodes);
        }

        public void ApplyAllBonuses(BonusManager manager)
        {
            foreach (var node in GetCompletedNodes())
            {
                node.ApplyBonuses(manager);
            }
        }
    }
}
