using NUnit.Framework;
using SlimeLab.Core;

namespace SlimeLab.Tests
{
    public class GeneTests
    {
        [Test]
        public void Gene_ShouldHaveIDAndName()
        {
            // Arrange
            string expectedName = "Fire Affinity";

            // Act
            var gene = new Gene(expectedName);

            // Assert
            Assert.IsNotNull(gene.ID);
            Assert.IsNotEmpty(gene.ID);
            Assert.AreEqual(expectedName, gene.Name);
        }

        [Test]
        public void Gene_ShouldHaveGeneType()
        {
            // Arrange & Act
            var dominantGene = new Gene("Strong Gene", GeneType.Dominant);
            var recessiveGene = new Gene("Weak Gene", GeneType.Recessive);

            // Assert
            Assert.AreEqual(GeneType.Dominant, dominantGene.Type);
            Assert.AreEqual(GeneType.Recessive, recessiveGene.Type);
        }

        [Test]
        public void Gene_DefaultsToRecessive()
        {
            // Arrange & Act
            var gene = new Gene("Test Gene");

            // Assert
            Assert.AreEqual(GeneType.Recessive, gene.Type);
        }
    }
}
