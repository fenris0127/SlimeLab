using NUnit.Framework;
using SlimeLab.Core;

namespace SlimeLab.Tests
{
    public class ResourceTests
    {
        [Test]
        public void ResourceType_EnumShouldExist()
        {
            // Arrange & Act
            var food = ResourceType.Food;
            var material = ResourceType.Material;
            var energy = ResourceType.Energy;
            var research = ResourceType.Research;

            // Assert
            Assert.AreEqual(ResourceType.Food, food);
            Assert.AreEqual(ResourceType.Material, material);
            Assert.AreEqual(ResourceType.Energy, energy);
            Assert.AreEqual(ResourceType.Research, research);
        }

        [Test]
        public void Resource_ShouldHaveTypeAndAmount()
        {
            // Arrange
            ResourceType expectedType = ResourceType.Food;
            int expectedAmount = 100;

            // Act
            var resource = new Resource(expectedType, expectedAmount);

            // Assert
            Assert.AreEqual(expectedType, resource.Type);
            Assert.AreEqual(expectedAmount, resource.Amount);
        }
    }
}
