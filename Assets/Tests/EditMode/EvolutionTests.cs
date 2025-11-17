using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    public class EvolutionTests
    {
        [Test]
        public void Slime_CanEvolveAtLevel10()
        {
            // Arrange
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(10);

            // Act
            bool canEvolve = slime.CanEvolve();

            // Assert
            Assert.IsTrue(canEvolve);
        }

        [Test]
        public void Slime_CannotEvolveBelowLevel10()
        {
            // Arrange
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(5);

            // Act
            bool canEvolve = slime.CanEvolve();

            // Assert
            Assert.IsFalse(canEvolve);
        }

        [Test]
        public void Slime_RequiresEvolutionItemToEvolve()
        {
            // Arrange
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Fire Stone", ElementType.Fire);

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                slime.Evolve(evolutionItem);
            });
        }

        [Test]
        public void Slime_CannotEvolveWithoutRequiredLevel()
        {
            // Arrange
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(5);
            var evolutionItem = new EvolutionItem("Fire Stone", ElementType.Fire);

            // Act & Assert
            Assert.Throws<System.InvalidOperationException>(() =>
            {
                slime.Evolve(evolutionItem);
            });
        }

        [Test]
        public void Slime_EvolutionIncreasesLevel()
        {
            // Arrange
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Fire Stone", ElementType.Fire);
            int initialLevel = slime.Level;

            // Act
            slime.Evolve(evolutionItem);

            // Assert
            Assert.Greater(slime.Level, initialLevel);
        }

        [Test]
        public void Slime_EvolutionIncreasesStats()
        {
            // Arrange
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Fire Stone", ElementType.Fire);
            int initialHP = slime.Stats.HP;
            int initialAttack = slime.Stats.Attack;

            // Act
            slime.Evolve(evolutionItem);

            // Assert
            Assert.Greater(slime.Stats.HP, initialHP);
            Assert.Greater(slime.Stats.Attack, initialAttack);
        }

        [Test]
        public void Slime_EvolutionChangesName()
        {
            // Arrange
            var slime = new Slime("Fire Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Fire Stone", ElementType.Fire);
            string originalName = slime.Name;

            // Act
            slime.Evolve(evolutionItem);

            // Assert
            Assert.AreNotEqual(originalName, slime.Name);
            Assert.IsTrue(slime.Name.Contains("Evolved") || slime.Name.Contains("Fire"));
        }

        [Test]
        public void EvolutionItem_HasNameAndElement()
        {
            // Arrange & Act
            var item = new EvolutionItem("Fire Stone", ElementType.Fire);

            // Assert
            Assert.AreEqual("Fire Stone", item.Name);
            Assert.AreEqual(ElementType.Fire, item.Element);
        }

        [Test]
        public void EvolutionTree_CanGetEvolutionPath()
        {
            // Arrange
            var tree = new EvolutionTree();
            var slime = new Slime("Fire Slime", ElementType.Fire);
            slime.SetLevel(10);

            // Act
            var evolutionPath = tree.GetEvolutionPath(slime);

            // Assert
            Assert.IsNotNull(evolutionPath);
        }

        [Test]
        public void EvolutionTree_HasMultiplePathsForDifferentElements()
        {
            // Arrange
            var tree = new EvolutionTree();
            var fireSlime = new Slime("Fire Slime", ElementType.Fire);
            var waterSlime = new Slime("Water Slime", ElementType.Water);
            fireSlime.SetLevel(10);
            waterSlime.SetLevel(10);

            // Act
            var firePath = tree.GetEvolutionPath(fireSlime);
            var waterPath = tree.GetEvolutionPath(waterSlime);

            // Assert
            Assert.IsNotNull(firePath);
            Assert.IsNotNull(waterPath);
            Assert.AreNotEqual(firePath.TargetName, waterPath.TargetName);
        }
    }
}
