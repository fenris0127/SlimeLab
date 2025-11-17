using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class AutoFeederTests
    {
        [Test]
        public void AutoFeeder_CanBeCreated()
        {
            var feeder = new AutoFeeder(feedInterval: 10);

            Assert.IsNotNull(feeder);
            Assert.AreEqual(10, feeder.FeedInterval);
        }

        [Test]
        public void AutoFeeder_HasDefaultFeedAmount()
        {
            var feeder = new AutoFeeder(feedInterval: 10);

            Assert.AreEqual(20, feeder.FeedAmount);
        }

        [Test]
        public void AutoFeeder_CanSetCustomFeedAmount()
        {
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 30);

            Assert.AreEqual(30, feeder.FeedAmount);
        }

        [Test]
        public void AutoFeeder_TracksTimeSinceLastFeed()
        {
            var feeder = new AutoFeeder(feedInterval: 10);

            Assert.AreEqual(0, feeder.TimeSinceLastFeed);
        }

        [Test]
        public void AutoFeeder_UpdateIncrementsTime()
        {
            var feeder = new AutoFeeder(feedInterval: 10);

            feeder.Update(5);

            Assert.AreEqual(5, feeder.TimeSinceLastFeed);
        }

        [Test]
        public void AutoFeeder_FeedsSlimeWhenIntervalReached()
        {
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 20);
            var slime = new Slime("Test Slime", ElementType.Neutral);
            slime.IncreaseHunger(50);

            int initialHunger = slime.Hunger;

            feeder.AttachSlime(slime);
            feeder.Update(10); // Reach the interval

            // Slime should have been fed
            Assert.Less(slime.Hunger, initialHunger);
        }

        [Test]
        public void AutoFeeder_ResetsTimerAfterFeeding()
        {
            var feeder = new AutoFeeder(feedInterval: 10);
            var slime = new Slime("Test Slime", ElementType.Neutral);

            feeder.AttachSlime(slime);
            feeder.Update(10);

            Assert.AreEqual(0, feeder.TimeSinceLastFeed);
        }

        [Test]
        public void AutoFeeder_CanFeedMultipleTimes()
        {
            var feeder = new AutoFeeder(feedInterval: 5, feedAmount: 10);
            var slime = new Slime("Test Slime", ElementType.Neutral);
            slime.IncreaseHunger(50);

            feeder.AttachSlime(slime);

            feeder.Update(5); // First feed
            int hungerAfterFirstFeed = slime.Hunger;

            slime.IncreaseHunger(20); // Increase hunger again
            feeder.Update(5); // Second feed

            Assert.Less(slime.Hunger, hungerAfterFirstFeed + 20);
        }

        [Test]
        public void AutoFeeder_ConsumesResourceWhenFeeding()
        {
            var feeder = new AutoFeeder(feedInterval: 10);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Food, 100));

            feeder.SetResourceInventory(inventory);

            var slime = new Slime("Test Slime", ElementType.Neutral);
            feeder.AttachSlime(slime);

            feeder.Update(10); // Trigger feeding

            // Should have consumed food resource
            Assert.Less(inventory.GetResourceAmount(ResourceType.Food), 100);
        }

        [Test]
        public void AutoFeeder_RequiresResourceToFeed()
        {
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 20);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Food, 5)); // Not enough

            feeder.SetResourceInventory(inventory);

            var slime = new Slime("Test Slime", ElementType.Neutral);
            slime.IncreaseHunger(50);

            feeder.AttachSlime(slime);

            int initialHunger = slime.Hunger;
            feeder.Update(10);

            // Slime should not have been fed (not enough resources)
            Assert.AreEqual(initialHunger, slime.Hunger);
        }

        [Test]
        public void AutoFeeder_ResourceConsumptionMatchesFeedAmount()
        {
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 25);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Food, 100));

            feeder.SetResourceInventory(inventory);

            var slime = new Slime("Test Slime", ElementType.Neutral);
            feeder.AttachSlime(slime);

            feeder.Update(10);

            // Should consume exactly feedAmount / 2 (conversion ratio)
            int expectedRemaining = 100 - 13; // 25/2 = 12.5, rounded up to 13
            Assert.AreEqual(expectedRemaining, inventory.GetResourceAmount(ResourceType.Food));
        }

        [Test]
        public void ContainmentUnit_CanAttachAutoFeeder()
        {
            var unit = new ContainmentUnit();
            var feeder = new AutoFeeder(feedInterval: 10);

            unit.AttachFeeder(feeder);

            Assert.IsNotNull(unit.Feeder);
            Assert.AreEqual(feeder, unit.Feeder);
        }

        [Test]
        public void ContainmentUnit_FeederAutomaticallyFeedsAssignedSlime()
        {
            var unit = new ContainmentUnit();
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 20);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Food, 100));

            feeder.SetResourceInventory(inventory);
            unit.AttachFeeder(feeder);

            var slime = new Slime("Test Slime", ElementType.Neutral);
            slime.IncreaseHunger(50);
            unit.AssignSlime(slime);

            int initialHunger = slime.Hunger;

            unit.Update(10); // Update unit, which should update feeder

            Assert.Less(slime.Hunger, initialHunger);
        }

        [Test]
        public void ContainmentUnit_CanDetachFeeder()
        {
            var unit = new ContainmentUnit();
            var feeder = new AutoFeeder(feedInterval: 10);

            unit.AttachFeeder(feeder);
            unit.DetachFeeder();

            Assert.IsNull(unit.Feeder);
        }

        [Test]
        public void AutoFeeder_DoesNotFeedIfNoSlimeAttached()
        {
            var feeder = new AutoFeeder(feedInterval: 10);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Food, 100));

            feeder.SetResourceInventory(inventory);

            feeder.Update(10);

            // Should not consume any resources
            Assert.AreEqual(100, inventory.GetResourceAmount(ResourceType.Food));
        }

        [Test]
        public void AutoFeeder_IsActiveByDefault()
        {
            var feeder = new AutoFeeder(feedInterval: 10);

            Assert.IsTrue(feeder.IsActive);
        }

        [Test]
        public void AutoFeeder_CanBeDeactivated()
        {
            var feeder = new AutoFeeder(feedInterval: 10);

            feeder.SetActive(false);

            Assert.IsFalse(feeder.IsActive);
        }

        [Test]
        public void AutoFeeder_DoesNotFeedWhenInactive()
        {
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 20);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Food, 100));

            feeder.SetResourceInventory(inventory);
            feeder.SetActive(false);

            var slime = new Slime("Test Slime", ElementType.Neutral);
            slime.IncreaseHunger(50);

            feeder.AttachSlime(slime);

            int initialHunger = slime.Hunger;
            feeder.Update(10);

            // Should not feed when inactive
            Assert.AreEqual(initialHunger, slime.Hunger);
            Assert.AreEqual(100, inventory.GetResourceAmount(ResourceType.Food));
        }

        [Test]
        public void AutoFeeder_CanBeReactivated()
        {
            var feeder = new AutoFeeder(feedInterval: 10, feedAmount: 20);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Food, 100));

            feeder.SetResourceInventory(inventory);
            feeder.SetActive(false);
            feeder.SetActive(true);

            var slime = new Slime("Test Slime", ElementType.Neutral);
            slime.IncreaseHunger(50);

            feeder.AttachSlime(slime);

            int initialHunger = slime.Hunger;
            feeder.Update(10);

            // Should feed after reactivation
            Assert.Less(slime.Hunger, initialHunger);
        }
    }
}
