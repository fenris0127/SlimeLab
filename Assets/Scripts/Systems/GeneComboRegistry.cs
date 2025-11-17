using System.Collections.Generic;
using System.Linq;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class GeneComboRegistry
    {
        private Dictionary<string, Gene> _combos;

        public GeneComboRegistry()
        {
            _combos = new Dictionary<string, Gene>();
            InitializeDefaultCombos();
        }

        public void RegisterCombo(List<string> geneNames, Gene resultGene)
        {
            string key = GenerateComboKey(geneNames);
            _combos[key] = resultGene;
        }

        public bool HasCombo(List<string> geneNames)
        {
            string key = GenerateComboKey(geneNames);
            return _combos.ContainsKey(key);
        }

        public Gene CheckForCombo(List<Gene> genes)
        {
            if (genes == null || genes.Count < 2)
            {
                return null;
            }

            var geneNames = genes.Select(g => g.Name).ToList();
            string key = GenerateComboKey(geneNames);

            if (_combos.ContainsKey(key))
            {
                var resultGene = _combos[key];
                // Return a new instance of the gene
                return new Gene(resultGene.Name, resultGene.Type);
            }

            return null;
        }

        private string GenerateComboKey(List<string> geneNames)
        {
            // Sort gene names alphabetically to ensure order doesn't matter
            var sortedNames = geneNames.OrderBy(n => n).ToList();
            return string.Join("|", sortedNames);
        }

        private void InitializeDefaultCombos()
        {
            // Fire + Speed = Blazing Speed
            RegisterCombo(
                new List<string> { "Fire Gene", "Speed Gene" },
                new Gene("Blazing Speed", GeneType.Dominant)
            );

            // Water + Defense = Aqua Shield
            RegisterCombo(
                new List<string> { "Water Gene", "Defense Gene" },
                new Gene("Aqua Shield", GeneType.Dominant)
            );

            // Electric + Speed = Lightning Dash
            RegisterCombo(
                new List<string> { "Electric Gene", "Speed Gene" },
                new Gene("Lightning Dash", GeneType.Dominant)
            );

            // Fire + Water + Electric = Tri-Element Master
            RegisterCombo(
                new List<string> { "Fire Gene", "Water Gene", "Electric Gene" },
                new Gene("Tri-Element Master", GeneType.Dominant)
            );

            // Fire + Water = Steam Power
            RegisterCombo(
                new List<string> { "Fire Gene", "Water Gene" },
                new Gene("Steam Power", GeneType.Dominant)
            );
        }
    }
}
