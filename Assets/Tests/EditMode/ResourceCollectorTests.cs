using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class ResourceCollectorTests
    {
        [Test]
        public void ResourceCollector_CanBeCreated()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            Assert.IsNotNull(collector);
            Assert.AreEqual(10, collector.CollectionInterval);
        }

        [Test]
        public void ResourceCollector_HasDefaultEfficiency()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            Assert.AreEqual(1.0f, collector.Efficiency);
        }

        [Test]
        public void ResourceCollector_HasDefaultLevel()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            Assert.AreEqual(1, collector.Level);
        }

        [Test]
        public void ResourceCollector_TracksTimeSinceLastCollection()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            Assert.AreEqual(0, collector.TimeSinceLastCollection);
        }

        [Test]
        public void ResourceCollector_UpdateIncrementsTime()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            collector.Update(5);

            Assert.AreEqual(5, collector.TimeSinceLastCollection);
        }

        [Test]
        public void ResourceCollector_CollectsResourceWhenIntervalReached()
        {
            var collector = new ResourceCollector(collectionInterval: 10);
            var inventory = new ResourceInventory();
            var slime = new Slime("Producer", ElementType.Fire);

            collector.SetResourceInventory(inventory);
            collector.AttachSlime(slime);

            collector.Update(10);

            // Should have collected some resource
            int totalResources = inventory.GetResourceAmount(ResourceType.Material) +
                                inventory.GetResourceAmount(ResourceType.Energy);
            Assert.Greater(totalResources, 0);
        }

        [Test]
        public void ResourceCollector_ResetsTimerAfterCollection()
        {
            var collector = new ResourceCollector(collectionInterval: 10);
            var inventory = new ResourceInventory();
            var slime = new Slime("Producer", ElementType.Fire);

            collector.SetResourceInventory(inventory);
            collector.AttachSlime(slime);

            collector.Update(10);

            Assert.AreEqual(0, collector.TimeSinceLastCollection);
        }

        [Test]
        public void ResourceCollector_CollectionAmountBasedOnSlimeLevel()
        {
            var collector = new ResourceCollector(collectionInterval: 10);
            var inventory1 = new ResourceInventory();
            var inventory2 = new ResourceInventory();

            var lowLevelSlime = new Slime("Newbie", ElementType.Fire);
            lowLevelSlime.SetLevel(1);

            var highLevelSlime = new Slime("Expert", ElementType.Fire);
            highLevelSlime.SetLevel(20);

            collector.SetResourceInventory(inventory1);
            collector.AttachSlime(lowLevelSlime);
            collector.Update(10);

            int lowLevelCollection = inventory1.GetResourceAmount(ResourceType.Material) +
                                     inventory1.GetResourceAmount(ResourceType.Energy);

            collector.DetachSlime();
            collector.SetResourceInventory(inventory2);
            collector.AttachSlime(highLevelSlime);
            collector.Update(10);

            int highLevelCollection = inventory2.GetResourceAmount(ResourceType.Material) +
                                      inventory2.GetResourceAmount(ResourceType.Energy);

            Assert.Greater(highLevelCollection, lowLevelCollection);
        }

        [Test]
        public void ResourceCollector_CanUpgrade()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            collector.Upgrade();

            Assert.AreEqual(2, collector.Level);
        }

        [Test]
        public void ResourceCollector_UpgradeIncreasesEfficiency()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            float initialEfficiency = collector.Efficiency;

            collector.Upgrade();

            Assert.Greater(collector.Efficiency, initialEfficiency);
        }

        [Test]
        public void ResourceCollector_EfficiencyAffectsCollection()
        {
            var collector1 = new ResourceCollector(collectionInterval: 10);
            var collector2 = new ResourceCollector(collectionInterval: 10);

            collector2.Upgrade();
            collector2.Upgrade();
            collector2.Upgrade(); // Level 4 collector

            var inventory1 = new ResourceInventory();
            var inventory2 = new ResourceInventory();

            var slime1 = new Slime("Slime 1", ElementType.Fire);
            slime1.SetLevel(10);
            var slime2 = new Slime("Slime 2", ElementType.Fire);
            slime2.SetLevel(10);

            collector1.SetResourceInventory(inventory1);
            collector1.AttachSlime(slime1);
            collector1.Update(10);

            collector2.SetResourceInventory(inventory2);
            collector2.AttachSlime(slime2);
            collector2.Update(10);

            int basicCollection = inventory1.GetResourceAmount(ResourceType.Material) +
                                  inventory1.GetResourceAmount(ResourceType.Energy);

            int upgradedCollection = inventory2.GetResourceAmount(ResourceType.Material) +
                                     inventory2.GetResourceAmount(ResourceType.Energy);

            Assert.Greater(upgradedCollection, basicCollection);
        }

        [Test]
        public void ResourceCollector_MaxLevel()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            // Upgrade to max level
            for (int i = 0; i < 10; i++)
            {
                collector.Upgrade();
            }

            Assert.LessOrEqual(collector.Level, 10);
        }

        [Test]
        public void ResourceCollector_CannotUpgradeBeyondMaxLevel()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            // Upgrade to max level
            for (int i = 0; i < 10; i++)
            {
                collector.Upgrade();
            }

            int maxLevel = collector.Level;

            collector.Upgrade(); // Try to upgrade beyond max

            Assert.AreEqual(maxLevel, collector.Level);
        }

        [Test]
        public void ResourceCollector_ElementalSlimesProduceDifferentResources()
        {
            var collectorFire = new ResourceCollector(collectionInterval: 10);
            var collectorWater = new ResourceCollector(collectionInterval: 10);

            var inventoryFire = new ResourceInventory();
            var inventoryWater = new ResourceInventory();

            var fireSlime = new Slime("Fire Slime", ElementType.Fire);
            var waterSlime = new Slime("Water Slime", ElementType.Water);

            collectorFire.SetResourceInventory(inventoryFire);
            collectorFire.AttachSlime(fireSlime);
            collectorFire.Update(10);

            collectorWater.SetResourceInventory(inventoryWater);
            collectorWater.AttachSlime(waterSlime);
            collectorWater.Update(10);

            // Fire slimes produce more Energy, Water slimes produce more Material
            Assert.Greater(inventoryFire.GetResourceAmount(ResourceType.Energy), 0);
            Assert.Greater(inventoryWater.GetResourceAmount(ResourceType.Material), 0);
        }

        [Test]
        public void ContainmentUnit_CanAttachCollector()
        {
            var unit = new ContainmentUnit();
            var collector = new ResourceCollector(collectionInterval: 10);

            unit.AttachCollector(collector);

            Assert.IsNotNull(unit.Collector);
            Assert.AreEqual(collector, unit.Collector);
        }

        [Test]
        public void ContainmentUnit_CollectorAutomaticallyCollectsFromAssignedSlime()
        {
            var unit = new ContainmentUnit();
            var collector = new ResourceCollector(collectionInterval: 10);
            var inventory = new ResourceInventory();

            collector.SetResourceInventory(inventory);
            unit.AttachCollector(collector);

            var slime = new Slime("Producer", ElementType.Fire);
            slime.SetLevel(10);
            unit.AssignSlime(slime);

            unit.Update(10);

            int totalResources = inventory.GetResourceAmount(ResourceType.Material) +
                                inventory.GetResourceAmount(ResourceType.Energy);
            Assert.Greater(totalResources, 0);
        }

        [Test]
        public void ContainmentUnit_CanDetachCollector()
        {
            var unit = new ContainmentUnit();
            var collector = new ResourceCollector(collectionInterval: 10);

            unit.AttachCollector(collector);
            unit.DetachCollector();

            Assert.IsNull(unit.Collector);
        }

        [Test]
        public void ResourceCollector_DoesNotCollectIfNoSlimeAttached()
        {
            var collector = new ResourceCollector(collectionInterval: 10);
            var inventory = new ResourceInventory();

            collector.SetResourceInventory(inventory);

            collector.Update(10);

            int totalResources = inventory.GetResourceAmount(ResourceType.Material) +
                                inventory.GetResourceAmount(ResourceType.Energy);
            Assert.AreEqual(0, totalResources);
        }

        [Test]
        public void ResourceCollector_IsActiveByDefault()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            Assert.IsTrue(collector.IsActive);
        }

        [Test]
        public void ResourceCollector_CanBeDeactivated()
        {
            var collector = new ResourceCollector(collectionInterval: 10);

            collector.SetActive(false);

            Assert.IsFalse(collector.IsActive);
        }

        [Test]
        public void ResourceCollector_DoesNotCollectWhenInactive()
        {
            var collector = new ResourceCollector(collectionInterval: 10);
            var inventory = new ResourceInventory();
            var slime = new Slime("Producer", ElementType.Fire);

            collector.SetResourceInventory(inventory);
            collector.AttachSlime(slime);
            collector.SetActive(false);

            collector.Update(10);

            int totalResources = inventory.GetResourceAmount(ResourceType.Material) +
                                inventory.GetResourceAmount(ResourceType.Energy);
            Assert.AreEqual(0, totalResources);
        }

        [Test]
        public void ContainmentUnit_UpdatesFeederAndCollectorTogether()
        {
            var unit = new ContainmentUnit();
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 20);
            var collector = new ResourceCollector(collectionInterval: 10);

            var feedInventory = new ResourceInventory();
            feedInventory.AddResource(new Resource(ResourceType.Food, 100));

            var collectInventory = new ResourceInventory();

            feeder.SetResourceInventory(feedInventory);
            collector.SetResourceInventory(collectInventory);

            unit.AttachFeeder(feeder);
            unit.AttachCollector(collector);

            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.IncreaseHunger(50);
            unit.AssignSlime(slime);

            int initialHunger = slime.Hunger;

            unit.Update(10);

            // Feeder should have fed slime
            Assert.Less(slime.Hunger, initialHunger);

            // Collector should have collected resources
            int totalCollected = collectInventory.GetResourceAmount(ResourceType.Material) +
                                collectInventory.GetResourceAmount(ResourceType.Energy);
            Assert.Greater(totalCollected, 0);
        }
    }
}
