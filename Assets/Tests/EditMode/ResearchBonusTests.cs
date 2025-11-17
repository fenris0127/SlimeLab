using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class ResearchBonusTests
    {
        [Test]
        public void PermanentBonus_CanBeCreated()
        {
            var bonus = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);

            Assert.IsNotNull(bonus);
            Assert.AreEqual(BonusType.BreedingSpeed, bonus.Type);
            Assert.AreEqual(0.1f, bonus.Value);
        }

        [Test]
        public void PermanentBonus_HasDescription()
        {
            var bonus = new PermanentBonus(BonusType.ResourceCollection, 0.15f, "Faster resource collection");

            Assert.AreEqual("Faster resource collection", bonus.Description);
        }

        [Test]
        public void TechNode_CanHavePermanentBonus()
        {
            var node = new TechNode("Advanced Breeding", "Improved breeding");
            var bonus = new PermanentBonus(BonusType.BreedingSpeed, 0.2f);

            node.AddBonus(bonus);

            Assert.AreEqual(1, node.Bonuses.Count);
            Assert.Contains(bonus, node.Bonuses);
        }

        [Test]
        public void TechNode_CanHaveMultipleBonuses()
        {
            var node = new TechNode("Super Research", "Multiple bonuses");
            var bonus1 = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);
            var bonus2 = new PermanentBonus(BonusType.ResourceCollection, 0.15f);

            node.AddBonus(bonus1);
            node.AddBonus(bonus2);

            Assert.AreEqual(2, node.Bonuses.Count);
        }

        [Test]
        public void BonusManager_CanApplyBonus()
        {
            var manager = new BonusManager();
            var bonus = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);

            manager.ApplyBonus(bonus);

            Assert.AreEqual(0.1f, manager.GetTotalBonus(BonusType.BreedingSpeed));
        }

        [Test]
        public void BonusManager_StacksMultipleBonuses()
        {
            var manager = new BonusManager();
            var bonus1 = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);
            var bonus2 = new PermanentBonus(BonusType.BreedingSpeed, 0.15f);

            manager.ApplyBonus(bonus1);
            manager.ApplyBonus(bonus2);

            Assert.AreEqual(0.25f, manager.GetTotalBonus(BonusType.BreedingSpeed));
        }

        [Test]
        public void BonusManager_TracksMultipleBonusTypes()
        {
            var manager = new BonusManager();
            var bonus1 = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);
            var bonus2 = new PermanentBonus(BonusType.ResourceCollection, 0.2f);
            var bonus3 = new PermanentBonus(BonusType.BreedingSpeed, 0.05f);

            manager.ApplyBonus(bonus1);
            manager.ApplyBonus(bonus2);
            manager.ApplyBonus(bonus3);

            Assert.AreEqual(0.15f, manager.GetTotalBonus(BonusType.BreedingSpeed));
            Assert.AreEqual(0.2f, manager.GetTotalBonus(BonusType.ResourceCollection));
        }

        [Test]
        public void BonusManager_ReturnsZeroForUnusedBonusType()
        {
            var manager = new BonusManager();

            Assert.AreEqual(0f, manager.GetTotalBonus(BonusType.ResearchSpeed));
        }

        [Test]
        public void TechNode_AppliesBonusesOnCompletion()
        {
            var node = new TechNode("Breeding Tech", "Improves breeding", researchTime: 10);
            var bonus = new PermanentBonus(BonusType.BreedingSpeed, 0.15f);
            var manager = new BonusManager();
            var inventory = new ResourceInventory();

            inventory.AddResource(new Resource(ResourceType.Research, 50));
            node.SetCost(ResourceType.Research, 50);
            node.AddBonus(bonus);

            node.StartResearch(inventory);
            node.UpdateResearch(10); // Complete research

            // Apply bonuses after completion
            node.ApplyBonuses(manager);

            Assert.AreEqual(0.15f, manager.GetTotalBonus(BonusType.BreedingSpeed));
        }

        [Test]
        public void BonusManager_CanRemoveBonus()
        {
            var manager = new BonusManager();
            var bonus = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);

            manager.ApplyBonus(bonus);
            Assert.AreEqual(0.1f, manager.GetTotalBonus(BonusType.BreedingSpeed));

            manager.RemoveBonus(bonus);
            Assert.AreEqual(0f, manager.GetTotalBonus(BonusType.BreedingSpeed));
        }

        [Test]
        public void BonusManager_CanGetActiveBonuses()
        {
            var manager = new BonusManager();
            var bonus1 = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);
            var bonus2 = new PermanentBonus(BonusType.ResourceCollection, 0.15f);

            manager.ApplyBonus(bonus1);
            manager.ApplyBonus(bonus2);

            var activeBonuses = manager.GetActiveBonuses();

            Assert.AreEqual(2, activeBonuses.Count);
            Assert.Contains(bonus1, activeBonuses);
            Assert.Contains(bonus2, activeBonuses);
        }

        [Test]
        public void BonusManager_CanGetBonusesByType()
        {
            var manager = new BonusManager();
            var bonus1 = new PermanentBonus(BonusType.BreedingSpeed, 0.1f);
            var bonus2 = new PermanentBonus(BonusType.BreedingSpeed, 0.15f);
            var bonus3 = new PermanentBonus(BonusType.ResourceCollection, 0.2f);

            manager.ApplyBonus(bonus1);
            manager.ApplyBonus(bonus2);
            manager.ApplyBonus(bonus3);

            var breedingBonuses = manager.GetBonusesByType(BonusType.BreedingSpeed);

            Assert.AreEqual(2, breedingBonuses.Count);
            Assert.Contains(bonus1, breedingBonuses);
            Assert.Contains(bonus2, breedingBonuses);
            Assert.IsFalse(breedingBonuses.Contains(bonus3));
        }

        [Test]
        public void TechTree_CanApplyAllCompletedBonuses()
        {
            var tree = new TechTree();
            var manager = new BonusManager();

            var node1 = new TechNode("Tech 1", "First tech");
            var node2 = new TechNode("Tech 2", "Second tech");

            node1.AddBonus(new PermanentBonus(BonusType.BreedingSpeed, 0.1f));
            node2.AddBonus(new PermanentBonus(BonusType.ResourceCollection, 0.15f));

            node1.CompleteResearch();
            node2.CompleteResearch();

            tree.AddNode(node1);
            tree.AddNode(node2);

            tree.ApplyAllBonuses(manager);

            Assert.AreEqual(0.1f, manager.GetTotalBonus(BonusType.BreedingSpeed));
            Assert.AreEqual(0.15f, manager.GetTotalBonus(BonusType.ResourceCollection));
        }

        [Test]
        public void TechTree_OnlyAppliesCompletedNodeBonuses()
        {
            var tree = new TechTree();
            var manager = new BonusManager();

            var completed = new TechNode("Completed", "Done");
            var inProgress = new TechNode("In Progress", "Not done");

            completed.AddBonus(new PermanentBonus(BonusType.BreedingSpeed, 0.1f));
            inProgress.AddBonus(new PermanentBonus(BonusType.ResourceCollection, 0.15f));

            completed.CompleteResearch();

            tree.AddNode(completed);
            tree.AddNode(inProgress);

            tree.ApplyAllBonuses(manager);

            Assert.AreEqual(0.1f, manager.GetTotalBonus(BonusType.BreedingSpeed));
            Assert.AreEqual(0f, manager.GetTotalBonus(BonusType.ResourceCollection));
        }

        [Test]
        public void BonusManager_CanCalculateMultiplier()
        {
            var manager = new BonusManager();
            manager.ApplyBonus(new PermanentBonus(BonusType.BreedingSpeed, 0.1f));
            manager.ApplyBonus(new PermanentBonus(BonusType.BreedingSpeed, 0.15f));

            float multiplier = manager.GetMultiplier(BonusType.BreedingSpeed);

            // 1.0 base + 0.25 bonus = 1.25 multiplier
            Assert.AreEqual(1.25f, multiplier);
        }

        [Test]
        public void BonusManager_DefaultMultiplierIsOne()
        {
            var manager = new BonusManager();

            float multiplier = manager.GetMultiplier(BonusType.ResearchSpeed);

            Assert.AreEqual(1.0f, multiplier);
        }

        [Test]
        public void PermanentBonus_CanBePercentage()
        {
            var bonus = new PermanentBonus(BonusType.BreedingSpeed, 0.25f, "25% faster breeding");

            Assert.AreEqual(0.25f, bonus.Value);
        }

        [Test]
        public void PermanentBonus_CanBeFlatValue()
        {
            var bonus = new PermanentBonus(BonusType.MaxCapacity, 5, "Add 5 capacity", isPercentage: false);

            Assert.IsFalse(bonus.IsPercentage);
            Assert.AreEqual(5, bonus.Value);
        }

        [Test]
        public void BonusManager_CanGetFlatBonusTotal()
        {
            var manager = new BonusManager();
            manager.ApplyBonus(new PermanentBonus(BonusType.MaxCapacity, 5, "", isPercentage: false));
            manager.ApplyBonus(new PermanentBonus(BonusType.MaxCapacity, 3, "", isPercentage: false));

            float total = manager.GetFlatBonus(BonusType.MaxCapacity);

            Assert.AreEqual(8, total);
        }

        [Test]
        public void BonusManager_SeparatesPercentageAndFlatBonuses()
        {
            var manager = new BonusManager();
            manager.ApplyBonus(new PermanentBonus(BonusType.BreedingSpeed, 0.1f)); // Percentage
            manager.ApplyBonus(new PermanentBonus(BonusType.BreedingSpeed, 2, "", isPercentage: false)); // Flat

            float percentageTotal = manager.GetTotalBonus(BonusType.BreedingSpeed);
            float flatTotal = manager.GetFlatBonus(BonusType.BreedingSpeed);

            Assert.AreEqual(0.1f, percentageTotal);
            Assert.AreEqual(2, flatTotal);
        }

        [Test]
        public void BonusType_HasAllExpectedTypes()
        {
            // Just validate enum exists
            var breedingSpeed = BonusType.BreedingSpeed;
            var resourceCollection = BonusType.ResourceCollection;
            var researchSpeed = BonusType.ResearchSpeed;
            var maxCapacity = BonusType.MaxCapacity;

            Assert.IsNotNull(breedingSpeed);
            Assert.IsNotNull(resourceCollection);
            Assert.IsNotNull(researchSpeed);
            Assert.IsNotNull(maxCapacity);
        }

        [Test]
        public void BonusManager_CanClearAllBonuses()
        {
            var manager = new BonusManager();
            manager.ApplyBonus(new PermanentBonus(BonusType.BreedingSpeed, 0.1f));
            manager.ApplyBonus(new PermanentBonus(BonusType.ResourceCollection, 0.15f));

            manager.ClearAll();

            Assert.AreEqual(0f, manager.GetTotalBonus(BonusType.BreedingSpeed));
            Assert.AreEqual(0f, manager.GetTotalBonus(BonusType.ResourceCollection));
            Assert.AreEqual(0, manager.GetActiveBonuses().Count);
        }
    }
}
