using NUnit.Framework;
using SlimeLab.Core;

namespace SlimeLab.Tests
{
    public class SlimeTests
    {
        [Test]
        public void Slime_ShouldHaveID()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.IsNotNull(slime.ID);
            Assert.IsNotEmpty(slime.ID);
        }

        [Test]
        public void Slime_ShouldHaveName()
        {
            // Arrange
            string expectedName = "Slimey";

            // Act
            var slime = new Slime(expectedName);

            // Assert
            Assert.IsNotNull(slime.Name);
            Assert.AreEqual(expectedName, slime.Name);
        }

        [Test]
        public void Slime_ShouldHaveElementType()
        {
            // Arrange & Act
            var fireSlime = new Slime("Fire Slime", ElementType.Fire);
            var waterSlime = new Slime("Water Slime", ElementType.Water);
            var electricSlime = new Slime("Electric Slime", ElementType.Electric);
            var neutralSlime = new Slime("Neutral Slime", ElementType.Neutral);

            // Assert
            Assert.AreEqual(ElementType.Fire, fireSlime.Element);
            Assert.AreEqual(ElementType.Water, waterSlime.Element);
            Assert.AreEqual(ElementType.Electric, electricSlime.Element);
            Assert.AreEqual(ElementType.Neutral, neutralSlime.Element);
        }

        [Test]
        public void Slime_ShouldHaveDefaultLevelOfOne()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.AreEqual(1, slime.Level);
        }

        [Test]
        public void Slime_ShouldHaveDefaultExperienceOfZero()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.AreEqual(0, slime.Experience);
        }
    }
}
