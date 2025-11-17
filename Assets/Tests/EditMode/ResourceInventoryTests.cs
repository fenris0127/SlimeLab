using NUnit.Framework;
using SlimeLab.Core;
using System;

namespace SlimeLab.Tests
{
    public class ResourceInventoryTests
    {
        [Test]
        public void ResourceInventory_CanAddResources()
        {
            // Arrange
            var inventory = new ResourceInventory();
            var foodResource = new Resource(ResourceType.Food, 50);

            // Act
            inventory.Add(foodResource);

            // Assert
            Assert.AreEqual(50, inventory.GetAmount(ResourceType.Food));
        }

        [Test]
        public void ResourceInventory_CanAddMultipleResourcesOfSameType()
        {
            // Arrange
            var inventory = new ResourceInventory();
            var food1 = new Resource(ResourceType.Food, 50);
            var food2 = new Resource(ResourceType.Food, 30);

            // Act
            inventory.Add(food1);
            inventory.Add(food2);

            // Assert
            Assert.AreEqual(80, inventory.GetAmount(ResourceType.Food));
        }

        [Test]
        public void ResourceInventory_CanConsumeResources()
        {
            // Arrange
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Material, 100));

            // Act
            inventory.Consume(ResourceType.Material, 30);

            // Assert
            Assert.AreEqual(70, inventory.GetAmount(ResourceType.Material));
        }

        [Test]
        public void ResourceInventory_ThrowsExceptionWhenInsufficientResources()
        {
            // Arrange
            var inventory = new ResourceInventory();
            inventory.Add(new Resource(ResourceType.Energy, 20));

            // Act & Assert
            Assert.Throws<InsufficientResourceException>(() =>
            {
                inventory.Consume(ResourceType.Energy, 50);
            });
        }

        [Test]
        public void ResourceInventory_ThrowsExceptionWhenConsumingNonExistentResource()
        {
            // Arrange
            var inventory = new ResourceInventory();

            // Act & Assert
            Assert.Throws<InsufficientResourceException>(() =>
            {
                inventory.Consume(ResourceType.Research, 10);
            });
        }
    }
}
