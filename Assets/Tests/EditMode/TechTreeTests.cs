using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class TechTreeTests
    {
        [Test]
        public void TechNode_CanBeCreated()
        {
            var node = new TechNode("Advanced Breeding", "Unlock advanced breeding features");

            Assert.IsNotNull(node);
            Assert.AreEqual("Advanced Breeding", node.Name);
            Assert.AreEqual("Unlock advanced breeding features", node.Description);
        }

        [Test]
        public void TechNode_HasResearchState()
        {
            var node = new TechNode("Test Tech", "Description");

            Assert.AreEqual(ResearchState.Locked, node.State);
        }

        [Test]
        public void TechNode_HasResearchCost()
        {
            var node = new TechNode("Test Tech", "Description");
            node.SetCost(ResourceType.Research, 100);

            Assert.AreEqual(100, node.GetCost(ResourceType.Research));
        }

        [Test]
        public void TechNode_HasResearchTime()
        {
            var node = new TechNode("Test Tech", "Description", researchTime: 50);

            Assert.AreEqual(50, node.ResearchTime);
        }

        [Test]
        public void TechNode_CanHavePrerequisites()
        {
            var prereq = new TechNode("Basic Tech", "Basic technology");
            var node = new TechNode("Advanced Tech", "Advanced technology");

            node.AddPrerequisite(prereq);

            Assert.AreEqual(1, node.Prerequisites.Count);
            Assert.Contains(prereq, node.Prerequisites);
        }

        [Test]
        public void TechNode_IsAvailableWhenPrerequisitesCompleted()
        {
            var prereq = new TechNode("Basic Tech", "Basic technology");
            var node = new TechNode("Advanced Tech", "Advanced technology");

            node.AddPrerequisite(prereq);

            // Initially not available
            Assert.IsFalse(node.IsAvailable());

            // Complete prerequisite
            prereq.CompleteResearch();

            // Now should be available
            Assert.IsTrue(node.IsAvailable());
        }

        [Test]
        public void TechNode_NotAvailableWhenPrerequisitesIncomplete()
        {
            var prereq = new TechNode("Basic Tech", "Basic technology");
            var node = new TechNode("Advanced Tech", "Advanced technology");

            node.AddPrerequisite(prereq);

            Assert.IsFalse(node.IsAvailable());
        }

        [Test]
        public void TechNode_AvailableWithNoPrerequisites()
        {
            var node = new TechNode("Starting Tech", "No prerequisites");

            Assert.IsTrue(node.IsAvailable());
        }

        [Test]
        public void TechNode_CanStartResearch()
        {
            var node = new TechNode("Test Tech", "Description", researchTime: 50);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 100));

            node.SetCost(ResourceType.Research, 50);

            node.StartResearch(inventory);

            Assert.AreEqual(ResearchState.InProgress, node.State);
            Assert.AreEqual(50, inventory.GetResourceAmount(ResourceType.Research));
        }

        [Test]
        public void TechNode_CannotStartResearchWithoutResources()
        {
            var node = new TechNode("Test Tech", "Description");
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 10));

            node.SetCost(ResourceType.Research, 50);

            Assert.Throws<System.InvalidOperationException>(() => node.StartResearch(inventory));
        }

        [Test]
        public void TechNode_CannotStartResearchWhenNotAvailable()
        {
            var prereq = new TechNode("Basic Tech", "Basic technology");
            var node = new TechNode("Advanced Tech", "Advanced technology");
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 100));

            node.AddPrerequisite(prereq);
            node.SetCost(ResourceType.Research, 50);

            Assert.Throws<System.InvalidOperationException>(() => node.StartResearch(inventory));
        }

        [Test]
        public void TechNode_ResearchProgressTracked()
        {
            var node = new TechNode("Test Tech", "Description", researchTime: 100);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 50));

            node.SetCost(ResourceType.Research, 50);
            node.StartResearch(inventory);

            Assert.AreEqual(0, node.ResearchProgress);

            node.UpdateResearch(30);
            Assert.AreEqual(30, node.ResearchProgress);

            node.UpdateResearch(40);
            Assert.AreEqual(70, node.ResearchProgress);
        }

        [Test]
        public void TechNode_CompletesWhenProgressReachesTime()
        {
            var node = new TechNode("Test Tech", "Description", researchTime: 50);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 50));

            node.SetCost(ResourceType.Research, 50);
            node.StartResearch(inventory);

            node.UpdateResearch(50);

            Assert.AreEqual(ResearchState.Completed, node.State);
        }

        [Test]
        public void TechNode_CanUnlockFeature()
        {
            var node = new TechNode("Test Tech", "Description");
            node.SetUnlockFeature("AdvancedBreeding");

            Assert.AreEqual("AdvancedBreeding", node.UnlockFeature);
        }

        [Test]
        public void TechNode_FeatureUnlockedWhenResearchCompleted()
        {
            var node = new TechNode("Test Tech", "Description", researchTime: 10);
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 50));

            node.SetCost(ResourceType.Research, 50);
            node.SetUnlockFeature("AdvancedBreeding");
            node.StartResearch(inventory);

            Assert.IsFalse(node.IsFeatureUnlocked());

            node.UpdateResearch(10);

            Assert.IsTrue(node.IsFeatureUnlocked());
        }

        [Test]
        public void TechNode_CanHaveMultipleCosts()
        {
            var node = new TechNode("Expensive Tech", "Costs multiple resources");

            node.SetCost(ResourceType.Research, 100);
            node.SetCost(ResourceType.Energy, 50);
            node.SetCost(ResourceType.Material, 25);

            Assert.AreEqual(100, node.GetCost(ResourceType.Research));
            Assert.AreEqual(50, node.GetCost(ResourceType.Energy));
            Assert.AreEqual(25, node.GetCost(ResourceType.Material));
        }

        [Test]
        public void TechNode_ConsumesAllRequiredResources()
        {
            var node = new TechNode("Expensive Tech", "Costs multiple resources");
            var inventory = new ResourceInventory();

            inventory.AddResource(new Resource(ResourceType.Research, 100));
            inventory.AddResource(new Resource(ResourceType.Energy, 50));

            node.SetCost(ResourceType.Research, 80);
            node.SetCost(ResourceType.Energy, 30);

            node.StartResearch(inventory);

            Assert.AreEqual(20, inventory.GetResourceAmount(ResourceType.Research));
            Assert.AreEqual(20, inventory.GetResourceAmount(ResourceType.Energy));
        }

        [Test]
        public void TechTree_CanManageMultipleTechNodes()
        {
            var tree = new TechTree();
            var node1 = new TechNode("Tech 1", "First tech");
            var node2 = new TechNode("Tech 2", "Second tech");

            tree.AddNode(node1);
            tree.AddNode(node2);

            Assert.AreEqual(2, tree.NodeCount);
        }

        [Test]
        public void TechTree_CanGetNodeByName()
        {
            var tree = new TechTree();
            var node = new TechNode("Test Tech", "Description");

            tree.AddNode(node);

            var retrieved = tree.GetNode("Test Tech");

            Assert.AreEqual(node, retrieved);
        }

        [Test]
        public void TechTree_CanGetAvailableNodes()
        {
            var tree = new TechTree();
            var available1 = new TechNode("Available 1", "No prereqs");
            var available2 = new TechNode("Available 2", "No prereqs");
            var locked = new TechNode("Locked", "Has prereqs");

            locked.AddPrerequisite(available1);

            tree.AddNode(available1);
            tree.AddNode(available2);
            tree.AddNode(locked);

            var availableNodes = tree.GetAvailableNodes();

            Assert.AreEqual(2, availableNodes.Count);
            Assert.Contains(available1, availableNodes);
            Assert.Contains(available2, availableNodes);
            Assert.IsFalse(availableNodes.Contains(locked));
        }

        [Test]
        public void TechTree_CanGetCompletedNodes()
        {
            var tree = new TechTree();
            var completed = new TechNode("Completed", "Finished");
            var inProgress = new TechNode("In Progress", "Working on it");

            completed.CompleteResearch();

            tree.AddNode(completed);
            tree.AddNode(inProgress);

            var completedNodes = tree.GetCompletedNodes();

            Assert.AreEqual(1, completedNodes.Count);
            Assert.Contains(completed, completedNodes);
        }

        [Test]
        public void TechNode_CannotStartAlreadyCompletedResearch()
        {
            var node = new TechNode("Test Tech", "Description");
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 100));

            node.SetCost(ResourceType.Research, 50);
            node.CompleteResearch();

            Assert.Throws<System.InvalidOperationException>(() => node.StartResearch(inventory));
        }

        [Test]
        public void TechNode_CannotStartAlreadyInProgressResearch()
        {
            var node = new TechNode("Test Tech", "Description");
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 100));

            node.SetCost(ResourceType.Research, 50);
            node.StartResearch(inventory);

            Assert.Throws<System.InvalidOperationException>(() => node.StartResearch(inventory));
        }

        [Test]
        public void TechTree_CanGetNodesInProgress()
        {
            var tree = new TechTree();
            var inProgress = new TechNode("In Progress", "Working on it", researchTime: 100);
            var locked = new TechNode("Locked", "Not started");
            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Research, 100));

            inProgress.SetCost(ResourceType.Research, 50);
            inProgress.StartResearch(inventory);

            tree.AddNode(inProgress);
            tree.AddNode(locked);

            var inProgressNodes = tree.GetNodesInProgress();

            Assert.AreEqual(1, inProgressNodes.Count);
            Assert.Contains(inProgress, inProgressNodes);
        }

        [Test]
        public void TechNode_HasUniqueID()
        {
            var node1 = new TechNode("Tech 1", "First");
            var node2 = new TechNode("Tech 2", "Second");

            Assert.AreNotEqual(node1.ID, node2.ID);
        }

        [Test]
        public void TechTree_CanGetUnlockedFeatures()
        {
            var tree = new TechTree();
            var node1 = new TechNode("Tech 1", "First");
            var node2 = new TechNode("Tech 2", "Second");

            node1.SetUnlockFeature("Feature1");
            node2.SetUnlockFeature("Feature2");

            node1.CompleteResearch();
            node2.CompleteResearch();

            tree.AddNode(node1);
            tree.AddNode(node2);

            var features = tree.GetUnlockedFeatures();

            Assert.AreEqual(2, features.Count);
            Assert.Contains("Feature1", features);
            Assert.Contains("Feature2", features);
        }

        [Test]
        public void TechTree_CanCheckIfFeatureIsUnlocked()
        {
            var tree = new TechTree();
            var node = new TechNode("Tech 1", "First");

            node.SetUnlockFeature("AdvancedBreeding");
            node.CompleteResearch();

            tree.AddNode(node);

            Assert.IsTrue(tree.IsFeatureUnlocked("AdvancedBreeding"));
            Assert.IsFalse(tree.IsFeatureUnlocked("NonExistentFeature"));
        }
    }
}
