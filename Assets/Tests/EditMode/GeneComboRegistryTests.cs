using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    public class GeneComboRegistryTests
    {
        [Test]
        public void GeneComboRegistry_CanRegisterCombo()
        {
            // Arrange
            var registry = new GeneComboRegistry();
            var geneNames = new List<string> { "Fire Gene", "Speed Gene" };
            var result = new Gene("Blazing Speed", GeneType.Dominant);

            // Act
            registry.RegisterCombo(geneNames, result);

            // Assert
            Assert.IsTrue(registry.HasCombo(geneNames));
        }

        [Test]
        public void GeneComboRegistry_ReturnsResultForMatchingCombo()
        {
            // Arrange
            var registry = new GeneComboRegistry();
            var geneNames = new List<string> { "Fire Gene", "Speed Gene" };
            var expectedResult = new Gene("Blazing Speed", GeneType.Dominant);
            registry.RegisterCombo(geneNames, expectedResult);

            var genes = new List<Gene>
            {
                new Gene("Fire Gene", GeneType.Dominant),
                new Gene("Speed Gene", GeneType.Recessive)
            };

            // Act
            var result = registry.CheckForCombo(genes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Name, result.Name);
        }

        [Test]
        public void GeneComboRegistry_ReturnsNullForNonMatchingCombo()
        {
            // Arrange
            var registry = new GeneComboRegistry();
            var geneNames = new List<string> { "Fire Gene", "Speed Gene" };
            var result = new Gene("Blazing Speed", GeneType.Dominant);
            registry.RegisterCombo(geneNames, result);

            var genes = new List<Gene>
            {
                new Gene("Water Gene", GeneType.Dominant),
                new Gene("Defense Gene", GeneType.Recessive)
            };

            // Act
            var comboResult = registry.CheckForCombo(genes);

            // Assert
            Assert.IsNull(comboResult);
        }

        [Test]
        public void GeneComboRegistry_IgnoresGeneOrder()
        {
            // Arrange
            var registry = new GeneComboRegistry();
            var geneNames = new List<string> { "Fire Gene", "Speed Gene" };
            var expectedResult = new Gene("Blazing Speed", GeneType.Dominant);
            registry.RegisterCombo(geneNames, expectedResult);

            var genes = new List<Gene>
            {
                new Gene("Speed Gene", GeneType.Recessive),
                new Gene("Fire Gene", GeneType.Dominant)
            };

            // Act
            var result = registry.CheckForCombo(genes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Name, result.Name);
        }

        [Test]
        public void GeneComboRegistry_WorksWithThreeGeneCombo()
        {
            // Arrange
            var registry = new GeneComboRegistry();
            var geneNames = new List<string> { "Fire Gene", "Water Gene", "Electric Gene" };
            var expectedResult = new Gene("Tri-Element Master", GeneType.Dominant);
            registry.RegisterCombo(geneNames, expectedResult);

            var genes = new List<Gene>
            {
                new Gene("Fire Gene", GeneType.Dominant),
                new Gene("Water Gene", GeneType.Dominant),
                new Gene("Electric Gene", GeneType.Dominant)
            };

            // Act
            var result = registry.CheckForCombo(genes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Name, result.Name);
        }
    }
}
