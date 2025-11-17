using System;
using System.Linq;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class BreedingChamber
    {
        public Slime Parent1 { get; private set; }
        public Slime Parent2 { get; private set; }
        public bool IsBreeding { get; private set; }
        public int BreedingDuration { get; private set; }
        public int BreedingProgress { get; private set; }

        private const int DEFAULT_BREEDING_DURATION = 100;
        private const int BREEDING_FOOD_COST = 50;
        private const float DEFAULT_MUTATION_RATE = 0.05f; // 5% chance

        private GeneComboRegistry _geneComboRegistry;
        private float _mutationRate;

        public BreedingChamber()
        {
            BreedingDuration = DEFAULT_BREEDING_DURATION;
            BreedingProgress = 0;
            IsBreeding = false;
            _mutationRate = DEFAULT_MUTATION_RATE;
            _geneComboRegistry = null;
        }

        public void SetGeneComboRegistry(GeneComboRegistry registry)
        {
            _geneComboRegistry = registry;
        }

        public void SetMutationRate(float rate)
        {
            _mutationRate = rate;
        }

        public void SetParents(Slime parent1, Slime parent2)
        {
            Parent1 = parent1;
            Parent2 = parent2;
        }

        public bool CheckCompatibility()
        {
            if (Parent1 == null || Parent2 == null)
            {
                return false;
            }

            // Check if parents have at least one gene with the same name
            foreach (var gene1 in Parent1.Genes)
            {
                foreach (var gene2 in Parent2.Genes)
                {
                    if (gene1.Name == gene2.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void StartBreeding(ResourceInventory inventory)
        {
            if (Parent1 == null || Parent2 == null)
            {
                throw new InvalidOperationException("Cannot start breeding without two parents");
            }

            if (!CheckCompatibility())
            {
                throw new InvalidOperationException("Parents are not compatible for breeding");
            }

            // Consume resources
            inventory.Consume(ResourceType.Food, BREEDING_FOOD_COST);

            IsBreeding = true;
            BreedingProgress = 0;
        }

        public void UpdateBreeding(int timeElapsed)
        {
            if (IsBreeding)
            {
                BreedingProgress += timeElapsed;
            }
        }

        public bool IsBreedingComplete()
        {
            return IsBreeding && BreedingProgress >= BreedingDuration;
        }

        public Slime CompleteBreeding()
        {
            if (!IsBreedingComplete())
            {
                throw new InvalidOperationException("Breeding is not yet complete");
            }

            // Create offspring
            var offspring = CreateOffspring();

            // Reset breeding state
            IsBreeding = false;
            BreedingProgress = 0;
            Parent1 = null;
            Parent2 = null;

            return offspring;
        }

        private Slime CreateOffspring()
        {
            // Determine offspring element (randomly from parents)
            Random random = new Random();
            ElementType offspringElement = random.Next(2) == 0 ? Parent1.Element : Parent2.Element;

            // Create new slime
            var offspring = new Slime($"Offspring of {Parent1.Name} & {Parent2.Name}", offspringElement);

            // Inherit genes from parents
            InheritGenes(offspring, Parent1, Parent2);

            return offspring;
        }

        private void InheritGenes(Slime offspring, Slime parent1, Slime parent2)
        {
            Random random = new Random();

            // Collect all parent genes
            var allParentGenes = parent1.Genes.Concat(parent2.Genes).ToList();

            // Group genes by name
            var geneGroups = allParentGenes.GroupBy(g => g.Name);

            foreach (var group in geneGroups)
            {
                var genesInGroup = group.ToList();

                // If there are dominant genes, prioritize them
                var dominantGenes = genesInGroup.Where(g => g.Type == GeneType.Dominant).ToList();

                Gene selectedGene;
                if (dominantGenes.Count > 0)
                {
                    // 75% chance to inherit dominant gene
                    if (random.Next(100) < 75)
                    {
                        selectedGene = dominantGenes[random.Next(dominantGenes.Count)];
                    }
                    else
                    {
                        selectedGene = genesInGroup[random.Next(genesInGroup.Count)];
                    }
                }
                else
                {
                    // All recessive, pick randomly
                    selectedGene = genesInGroup[random.Next(genesInGroup.Count)];
                }

                // Create a new gene instance for the offspring
                offspring.AddGene(new Gene(selectedGene.Name, selectedGene.Type));
            }

            // Check for gene combinations
            if (_geneComboRegistry != null)
            {
                var comboGene = _geneComboRegistry.CheckForCombo(offspring.Genes);
                if (comboGene != null)
                {
                    offspring.AddGene(comboGene);
                }
            }

            // Apply mutation
            if (random.NextDouble() < _mutationRate)
            {
                ApplyMutation(offspring, random);
            }
        }

        private void ApplyMutation(Slime offspring, Random random)
        {
            // Generate a random mutation gene
            string[] mutationTypes = new string[]
            {
                "Mutation Alpha",
                "Mutation Beta",
                "Mutation Gamma",
                "Mutation Delta",
                "Mutation Omega"
            };

            string mutationName = mutationTypes[random.Next(mutationTypes.Length)];
            GeneType mutationType = random.Next(2) == 0 ? GeneType.Dominant : GeneType.Recessive;

            offspring.AddGene(new Gene(mutationName, mutationType));
        }
    }
}
