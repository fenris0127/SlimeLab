using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class ResourceCollectionTests
    {
        [Test]
        public void ResourceNode_CanBeCreatedInZone()
        {
            var resourceNode = new ResourceNode(ResourceType.Material, 100);

            Assert.IsNotNull(resourceNode);
            Assert.AreEqual(ResourceType.Material, resourceNode.ResourceType);
            Assert.AreEqual(100, resourceNode.Amount);
        }

        [Test]
        public void ResourceNode_HasPosition()
        {
            var resourceNode = new ResourceNode(ResourceType.Food, 50, x: 5, y: 3);

            Assert.AreEqual(5, resourceNode.X);
            Assert.AreEqual(3, resourceNode.Y);
        }

        [Test]
        public void Zone_CanContainResourceNodes()
        {
            var zone = new Zone("Resource Zone", 2);
            var node1 = new ResourceNode(ResourceType.Material, 100, 3, 4);
            var node2 = new ResourceNode(ResourceType.Food, 50, 7, 2);

            zone.AddResourceNode(node1);
            zone.AddResourceNode(node2);

            Assert.AreEqual(2, zone.ResourceNodeCount);
        }

        [Test]
        public void Zone_CanGetResourceNodeAtPosition()
        {
            var zone = new Zone("Resource Zone", 2);
            var node = new ResourceNode(ResourceType.Energy, 75, 5, 5);

            zone.AddResourceNode(node);

            var foundNode = zone.GetResourceNodeAt(5, 5);

            Assert.IsNotNull(foundNode);
            Assert.AreEqual(ResourceType.Energy, foundNode.ResourceType);
        }

        [Test]
        public void Slime_CanGatherResourceFromNode()
        {
            var slime = new Slime("Gatherer", ElementType.Neutral);
            slime.SetLevel(5);
            var node = new ResourceNode(ResourceType.Material, 100);

            var gathered = slime.GatherResource(node);

            Assert.IsNotNull(gathered);
            Assert.AreEqual(ResourceType.Material, gathered.Type);
            Assert.Greater(gathered.Amount, 0);
        }

        [Test]
        public void Slime_GatheringEfficiencyBasedOnLevel()
        {
            var lowLevelSlime = new Slime("Newbie", ElementType.Neutral);
            lowLevelSlime.SetLevel(1);

            var highLevelSlime = new Slime("Expert", ElementType.Neutral);
            highLevelSlime.SetLevel(20);

            var node1 = new ResourceNode(ResourceType.Material, 100);
            var node2 = new ResourceNode(ResourceType.Material, 100);

            var gathered1 = lowLevelSlime.GatherResource(node1);
            var gathered2 = highLevelSlime.GatherResource(node2);

            // Higher level slime should gather more
            Assert.Greater(gathered2.Amount, gathered1.Amount);
        }

        [Test]
        public void ResourceNode_DepletesWhenGathered()
        {
            var slime = new Slime("Gatherer", ElementType.Neutral);
            slime.SetLevel(10);
            var node = new ResourceNode(ResourceType.Food, 50);

            int initialAmount = node.Amount;
            slime.GatherResource(node);

            Assert.Less(node.Amount, initialAmount);
        }

        [Test]
        public void ResourceNode_CanBeDepleted()
        {
            var slime = new Slime("Gatherer", ElementType.Neutral);
            slime.SetLevel(20);
            var node = new ResourceNode(ResourceType.Material, 10);

            // Gather until depleted
            while (node.Amount > 0)
            {
                slime.GatherResource(node);
            }

            Assert.IsTrue(node.IsDepleted());
            Assert.AreEqual(0, node.Amount);
        }

        [Test]
        public void Slime_CannotGatherFromDepletedNode()
        {
            var slime = new Slime("Gatherer", ElementType.Neutral);
            var node = new ResourceNode(ResourceType.Energy, 0);

            var gathered = slime.GatherResource(node);

            Assert.IsNull(gathered);
        }

        [Test]
        public void Expedition_CanCollectResources()
        {
            var zone = new Zone("Resource Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Gatherer", ElementType.Neutral);

            expedition.AddSlime(slime);

            var resource = new Resource(ResourceType.Material, 50);
            expedition.AddCollectedResource(resource);

            Assert.AreEqual(1, expedition.CollectedResources.Count);
            Assert.AreEqual(50, expedition.CollectedResources[0].Amount);
        }

        [Test]
        public void Expedition_CanCompleteAndReturnResources()
        {
            var lab = new Laboratory("Test Lab");
            var zone = new Zone("Resource Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Gatherer", ElementType.Neutral);

            lab.AddSlime(slime);
            expedition.AddSlime(slime);
            expedition.Start(lab);

            // Simulate resource collection
            var resource = new Resource(ResourceType.Material, 100);
            expedition.AddCollectedResource(resource);

            var collectedResources = expedition.Complete(lab);

            Assert.AreEqual(ExpeditionStatus.Completed, expedition.Status);
            Assert.AreEqual(1, collectedResources.Count);
            Assert.AreEqual(100, collectedResources[0].Amount);
        }

        [Test]
        public void Expedition_ReturnsSlimesToLaboratoryOnComplete()
        {
            var lab = new Laboratory("Test Lab");
            var zone = new Zone("Resource Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Gatherer", ElementType.Neutral);

            lab.AddSlime(slime);
            expedition.AddSlime(slime);
            expedition.Start(lab);

            Assert.AreEqual(0, lab.SlimeCount);

            expedition.Complete(lab);

            // Slime should be returned to lab
            Assert.AreEqual(1, lab.SlimeCount);
            Assert.IsTrue(lab.ContainsSlime(slime.ID));
        }

        [Test]
        public void Expedition_CannotCompleteIfNotActive()
        {
            var lab = new Laboratory("Test Lab");
            var zone = new Zone("Resource Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Gatherer", ElementType.Neutral);

            lab.AddSlime(slime);
            expedition.AddSlime(slime);

            // Don't start expedition
            Assert.Throws<System.InvalidOperationException>(() => expedition.Complete(lab));
        }

        [Test]
        public void Slime_ElementalAffinityAffectsGatheringEfficiency()
        {
            var fireSlime = new Slime("Fire Gatherer", ElementType.Fire);
            var waterSlime = new Slime("Water Gatherer", ElementType.Water);

            fireSlime.SetLevel(10);
            waterSlime.SetLevel(10);

            // Fire slime gathering in Volcanic environment
            var volcanicNode = new ResourceNode(ResourceType.Energy, 100);
            volcanicNode.SetEnvironmentBonus(ElementType.Fire);

            var gathered = fireSlime.GatherResource(volcanicNode);

            // Fire slime should get bonus in fire-aligned node
            Assert.Greater(gathered.Amount, 10); // Base would be around 10
        }

        [Test]
        public void Expedition_TracksMultipleResourceTypes()
        {
            var zone = new Zone("Resource Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Gatherer", ElementType.Neutral);

            expedition.AddSlime(slime);

            expedition.AddCollectedResource(new Resource(ResourceType.Material, 50));
            expedition.AddCollectedResource(new Resource(ResourceType.Food, 30));
            expedition.AddCollectedResource(new Resource(ResourceType.Energy, 20));

            Assert.AreEqual(3, expedition.CollectedResources.Count);
        }

        [Test]
        public void Expedition_CombinesSameResourceTypes()
        {
            var zone = new Zone("Resource Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Gatherer", ElementType.Neutral);

            expedition.AddSlime(slime);

            expedition.AddCollectedResource(new Resource(ResourceType.Material, 50));
            expedition.AddCollectedResource(new Resource(ResourceType.Material, 30));

            // Should combine into single resource entry
            var materialResources = expedition.GetResourcesByType(ResourceType.Material);
            Assert.AreEqual(80, materialResources);
        }
    }
}
