using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using SlimeLab.UI;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class UIComponentTests
    {
        [Test]
        public void SlimeCardData_CanBeCreated()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            var cardData = new SlimeCardData(slime);

            Assert.IsNotNull(cardData);
        }

        [Test]
        public void SlimeCardData_DisplaysSlimeName()
        {
            var slime = new Slime("FireSlime", ElementType.Fire);
            var cardData = new SlimeCardData(slime);

            Assert.AreEqual("FireSlime", cardData.Name);
        }

        [Test]
        public void SlimeCardData_DisplaysSlimeElement()
        {
            var slime = new Slime("WaterSlime", ElementType.Water);
            var cardData = new SlimeCardData(slime);

            Assert.AreEqual(ElementType.Water, cardData.Element);
        }

        [Test]
        public void SlimeCardData_DisplaysSlimeLevel()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            slime.GainExperience(100);

            var cardData = new SlimeCardData(slime);

            Assert.AreEqual(slime.Level, cardData.Level);
        }

        [Test]
        public void SlimeCardData_DisplaysSlimeStats()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            var cardData = new SlimeCardData(slime);

            Assert.AreEqual(slime.Stats.HP, cardData.HP);
            Assert.AreEqual(slime.Stats.MaxHP, cardData.MaxHP);
            Assert.AreEqual(slime.Stats.Attack, cardData.Attack);
            Assert.AreEqual(slime.Stats.Defense, cardData.Defense);
        }

        [Test]
        public void SlimeCardData_DisplaysHungerLevel()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            slime.Feed(50);

            var cardData = new SlimeCardData(slime);

            Assert.AreEqual(slime.Hunger, cardData.Hunger);
        }

        [Test]
        public void SlimeCardData_CanRefreshFromSlime()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            var cardData = new SlimeCardData(slime);

            Assert.AreEqual(1, cardData.Level);

            // Modify slime
            slime.GainExperience(100);

            // Refresh card data
            cardData.Refresh(slime);

            Assert.AreEqual(slime.Level, cardData.Level);
        }

        [Test]
        public void ResourceBarData_CanBeCreated()
        {
            var barData = new ResourceBarData(ResourceType.Energy, 50, 100);

            Assert.IsNotNull(barData);
        }

        [Test]
        public void ResourceBarData_DisplaysResourceType()
        {
            var barData = new ResourceBarData(ResourceType.Energy, 50, 100);

            Assert.AreEqual(ResourceType.Energy, barData.ResourceType);
        }

        [Test]
        public void ResourceBarData_DisplaysCurrentAmount()
        {
            var barData = new ResourceBarData(ResourceType.Food, 75, 150);

            Assert.AreEqual(75, barData.CurrentAmount);
        }

        [Test]
        public void ResourceBarData_DisplaysMaxAmount()
        {
            var barData = new ResourceBarData(ResourceType.Material, 30, 200);

            Assert.AreEqual(200, barData.MaxAmount);
        }

        [Test]
        public void ResourceBarData_CalculatesPercentage()
        {
            var barData = new ResourceBarData(ResourceType.Energy, 50, 100);

            Assert.AreEqual(0.5f, barData.Percentage);
        }

        [Test]
        public void ResourceBarData_HandlesZeroMax()
        {
            var barData = new ResourceBarData(ResourceType.Energy, 0, 0);

            Assert.AreEqual(0f, barData.Percentage);
        }

        [Test]
        public void ResourceBarData_CanUpdate()
        {
            var barData = new ResourceBarData(ResourceType.Energy, 50, 100);

            Assert.AreEqual(50, barData.CurrentAmount);

            barData.Update(75, 100);

            Assert.AreEqual(75, barData.CurrentAmount);
            Assert.AreEqual(0.75f, barData.Percentage);
        }

        [Test]
        public void LabViewData_CanBeCreated()
        {
            var laboratory = new Laboratory(10);
            var viewData = new LabViewData(laboratory);

            Assert.IsNotNull(viewData);
        }

        [Test]
        public void LabViewData_DisplaysCapacity()
        {
            var laboratory = new Laboratory(15);
            var viewData = new LabViewData(laboratory);

            Assert.AreEqual(15, viewData.Capacity);
        }

        [Test]
        public void LabViewData_DisplaysCurrentSlimeCount()
        {
            var laboratory = new Laboratory(10);
            laboratory.AddSlime(new Slime("Slime1", ElementType.Fire));
            laboratory.AddSlime(new Slime("Slime2", ElementType.Water));

            var viewData = new LabViewData(laboratory);

            Assert.AreEqual(2, viewData.CurrentSlimeCount);
        }

        [Test]
        public void LabViewData_DisplaysAvailableSpace()
        {
            var laboratory = new Laboratory(10);
            laboratory.AddSlime(new Slime("Slime1", ElementType.Fire));
            laboratory.AddSlime(new Slime("Slime2", ElementType.Water));

            var viewData = new LabViewData(laboratory);

            Assert.AreEqual(8, viewData.AvailableSpace);
        }

        [Test]
        public void LabViewData_DisplaysOccupancyPercentage()
        {
            var laboratory = new Laboratory(10);
            laboratory.AddSlime(new Slime("Slime1", ElementType.Fire));
            laboratory.AddSlime(new Slime("Slime2", ElementType.Water));
            laboratory.AddSlime(new Slime("Slime3", ElementType.Electric));

            var viewData = new LabViewData(laboratory);

            Assert.AreEqual(0.3f, viewData.OccupancyPercentage);
        }

        [Test]
        public void LabViewData_CanRefresh()
        {
            var laboratory = new Laboratory(10);
            var viewData = new LabViewData(laboratory);

            Assert.AreEqual(0, viewData.CurrentSlimeCount);

            laboratory.AddSlime(new Slime("Slime1", ElementType.Fire));

            viewData.Refresh(laboratory);

            Assert.AreEqual(1, viewData.CurrentSlimeCount);
        }

        [Test]
        public void LabViewData_ProvidesSlimeList()
        {
            var laboratory = new Laboratory(10);
            var slime1 = new Slime("Slime1", ElementType.Fire);
            var slime2 = new Slime("Slime2", ElementType.Water);

            laboratory.AddSlime(slime1);
            laboratory.AddSlime(slime2);

            var viewData = new LabViewData(laboratory);
            var slimes = viewData.GetSlimes();

            Assert.AreEqual(2, slimes.Count);
            Assert.Contains(slime1, slimes);
            Assert.Contains(slime2, slimes);
        }

        [Test]
        public void ResourceBarCollection_CanCreateFromInventory()
        {
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Energy, 50));
            inventory.AddResource(new Resource(ResourceType.Food, 75));

            var collection = new ResourceBarCollection(inventory);

            Assert.IsNotNull(collection);
        }

        [Test]
        public void ResourceBarCollection_CreatesBarForEachResourceType()
        {
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Energy, 50));
            inventory.AddResource(new Resource(ResourceType.Food, 75));
            inventory.AddResource(new Resource(ResourceType.Material, 30));

            var collection = new ResourceBarCollection(inventory);

            Assert.AreEqual(3, collection.GetBars().Count);
        }

        [Test]
        public void ResourceBarCollection_CanGetBarByType()
        {
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Energy, 50));

            var collection = new ResourceBarCollection(inventory);
            var energyBar = collection.GetBar(ResourceType.Energy);

            Assert.IsNotNull(energyBar);
            Assert.AreEqual(ResourceType.Energy, energyBar.ResourceType);
            Assert.AreEqual(50, energyBar.CurrentAmount);
        }

        [Test]
        public void ResourceBarCollection_CanRefresh()
        {
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Energy, 50));

            var collection = new ResourceBarCollection(inventory);
            var energyBar = collection.GetBar(ResourceType.Energy);

            Assert.AreEqual(50, energyBar.CurrentAmount);

            // Modify inventory
            inventory.AddResource(new Resource(ResourceType.Energy, 25));

            // Refresh collection
            collection.Refresh(inventory);

            energyBar = collection.GetBar(ResourceType.Energy);
            Assert.AreEqual(75, energyBar.CurrentAmount);
        }

        [Test]
        public void SlimeCardData_DisplaysMoodStatus()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            slime.Feed(100);

            var cardData = new SlimeCardData(slime);

            Assert.AreEqual(slime.Mood, cardData.Mood);
        }

        [Test]
        public void SlimeCardData_CanGenerateDisplayText()
        {
            var slime = new Slime("FireSlime", ElementType.Fire);
            var cardData = new SlimeCardData(slime);

            string displayText = cardData.GetDisplayText();

            Assert.IsNotNull(displayText);
            Assert.IsTrue(displayText.Contains("FireSlime"));
            Assert.IsTrue(displayText.Contains("Fire"));
        }

        [Test]
        public void LabViewData_IndicatesWhenFull()
        {
            var laboratory = new Laboratory(2);
            laboratory.AddSlime(new Slime("Slime1", ElementType.Fire));
            laboratory.AddSlime(new Slime("Slime2", ElementType.Water));

            var viewData = new LabViewData(laboratory);

            Assert.IsTrue(viewData.IsFull);
        }

        [Test]
        public void LabViewData_IndicatesWhenNotFull()
        {
            var laboratory = new Laboratory(10);
            laboratory.AddSlime(new Slime("Slime1", ElementType.Fire));

            var viewData = new LabViewData(laboratory);

            Assert.IsFalse(viewData.IsFull);
        }

        [Test]
        public void ResourceBarData_CanSetMaxAmount()
        {
            var barData = new ResourceBarData(ResourceType.Energy, 50, 100);

            Assert.AreEqual(100, barData.MaxAmount);

            barData.SetMaxAmount(200);

            Assert.AreEqual(200, barData.MaxAmount);
            Assert.AreEqual(0.25f, barData.Percentage); // 50/200
        }

        [Test]
        public void SlimeCardData_ShowsExperienceProgress()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            slime.GainExperience(50);

            var cardData = new SlimeCardData(slime);

            Assert.AreEqual(slime.Experience, cardData.Experience);
            Assert.AreEqual(slime.ExperienceToNextLevel, cardData.ExperienceToNextLevel);
        }

        [Test]
        public void LabViewData_ProvidesContainmentUnitData()
        {
            var laboratory = new Laboratory(3);
            var unit1 = new ContainmentUnit(1);
            var unit2 = new ContainmentUnit(2);

            laboratory.AddContainmentUnit(unit1);
            laboratory.AddContainmentUnit(unit2);

            var viewData = new LabViewData(laboratory);
            var units = viewData.GetContainmentUnits();

            Assert.AreEqual(2, units.Count);
            Assert.Contains(unit1, units);
            Assert.Contains(unit2, units);
        }

        [Test]
        public void ResourceBarCollection_HandlesEmptyInventory()
        {
            var inventory = new ResourceInventory();
            var collection = new ResourceBarCollection(inventory);

            Assert.AreEqual(0, collection.GetBars().Count);
        }
    }
}
