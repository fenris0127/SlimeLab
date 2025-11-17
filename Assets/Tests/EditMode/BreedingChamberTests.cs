using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    public class BreedingChamberTests
    {
        [Test]
        public void BreedingChamber_CanAcceptTwoSlimes()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var slime1 = new Slime("Parent 1", ElementType.Fire);
            var slime2 = new Slime("Parent 2", ElementType.Water);

            // Act
            chamber.SetParents(slime1, slime2);

            // Assert
            Assert.AreEqual(slime1.ID, chamber.Parent1.ID);
            Assert.AreEqual(slime2.ID, chamber.Parent2.ID);
        }

        [Test]
        public void BreedingChamber_ChecksCompatibility()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var slime1 = new Slime("Adult 1", ElementType.Fire);
            var slime2 = new Slime("Adult 2", ElementType.Water);

            slime1.AddGene(new Gene("Compatible Gene", GeneType.Dominant));
            slime2.AddGene(new Gene("Compatible Gene", GeneType.Dominant));

            // Act
            chamber.SetParents(slime1, slime2);
            bool isCompatible = chamber.CheckCompatibility();

            // Assert
            Assert.IsTrue(isCompatible);
        }

        [Test]
        public void BreedingChamber_IncompatibleSlimesCannotBreed()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var slime1 = new Slime("Slime 1", ElementType.Fire);
            var slime2 = new Slime("Baby Slime", ElementType.Water);

            // Slimes with no common genes are incompatible
            slime1.AddGene(new Gene("Fire Gene", GeneType.Dominant));
            slime2.AddGene(new Gene("Water Gene", GeneType.Dominant));

            chamber.SetParents(slime1, slime2);

            // Act
            bool isCompatible = chamber.CheckCompatibility();

            // Assert
            Assert.IsFalse(isCompatible);
        }

        [Test]
        public void BreedingChamber_RequiresResourceToStartBreeding()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Food, 100));

            var slime1 = new Slime("Parent 1");
            var slime2 = new Slime("Parent 2");
            slime1.AddGene(new Gene("Common Gene"));
            slime2.AddGene(new Gene("Common Gene"));

            chamber.SetParents(slime1, slime2);

            // Act
            chamber.StartBreeding(inventory);

            // Assert
            Assert.IsTrue(chamber.IsBreeding);
            Assert.Less(inventory.GetAmount(ResourceType.Food), 100);
        }

        [Test]
        public void BreedingChamber_ThrowsExceptionWhenInsufficientResources()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Food, 10)); // Not enough

            var slime1 = new Slime("Parent 1");
            var slime2 = new Slime("Parent 2");
            slime1.AddGene(new Gene("Common Gene"));
            slime2.AddGene(new Gene("Common Gene"));

            chamber.SetParents(slime1, slime2);

            // Act & Assert
            Assert.Throws<InsufficientResourceException>(() =>
            {
                chamber.StartBreeding(inventory);
            });
        }

        [Test]
        public void BreedingChamber_HasBreedingDuration()
        {
            // Arrange
            var chamber = new BreedingChamber();

            // Act
            int duration = chamber.BreedingDuration;

            // Assert
            Assert.Greater(duration, 0);
        }

        [Test]
        public void BreedingChamber_TracksBreedingProgress()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Food, 100));

            var slime1 = new Slime("Parent 1");
            var slime2 = new Slime("Parent 2");
            slime1.AddGene(new Gene("Common Gene"));
            slime2.AddGene(new Gene("Common Gene"));

            chamber.SetParents(slime1, slime2);
            chamber.StartBreeding(inventory);

            // Act
            chamber.UpdateBreeding(10);

            // Assert
            Assert.AreEqual(10, chamber.BreedingProgress);
        }

        [Test]
        public void BreedingChamber_CompletesBreedingAfterDuration()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Food, 100));

            var slime1 = new Slime("Parent 1", ElementType.Fire);
            var slime2 = new Slime("Parent 2", ElementType.Water);
            slime1.AddGene(new Gene("Common Gene"));
            slime2.AddGene(new Gene("Common Gene"));

            chamber.SetParents(slime1, slime2);
            chamber.StartBreeding(inventory);

            // Act
            chamber.UpdateBreeding(chamber.BreedingDuration);
            bool isComplete = chamber.IsBreedingComplete();

            // Assert
            Assert.IsTrue(isComplete);
        }

        [Test]
        public void BreedingChamber_ProducesOffspring()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Food, 100));

            var slime1 = new Slime("Parent 1", ElementType.Fire);
            var slime2 = new Slime("Parent 2", ElementType.Water);
            slime1.AddGene(new Gene("Fire Gene", GeneType.Dominant));
            slime2.AddGene(new Gene("Water Gene", GeneType.Recessive));

            chamber.SetParents(slime1, slime2);
            chamber.StartBreeding(inventory);
            chamber.UpdateBreeding(chamber.BreedingDuration);

            // Act
            var offspring = chamber.CompleteBreeding();

            // Assert
            Assert.IsNotNull(offspring);
            Assert.IsNotEmpty(offspring.ID);
            Assert.Greater(offspring.Genes.Count, 0);
        }

        [Test]
        public void BreedingChamber_CannotStartBreedingWithoutParents()
        {
            // Arrange
            var chamber = new BreedingChamber();
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Food, 100));

            // Act & Assert
            Assert.Throws<System.InvalidOperationException>(() =>
            {
                chamber.StartBreeding(inventory);
            });
        }
    }
}
