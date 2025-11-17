using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class ExpeditionTests
    {
        [Test]
        public void Expedition_CanBeCreated()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);

            Assert.IsNotNull(expedition);
            Assert.AreEqual(zone, expedition.TargetZone);
        }

        [Test]
        public void Expedition_CanAddSlimeToTeam()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Test Slime", ElementType.Fire);

            expedition.AddSlime(slime);

            Assert.AreEqual(1, expedition.TeamSize);
            Assert.IsTrue(expedition.HasSlime(slime.ID));
        }

        [Test]
        public void Expedition_CanAddMultipleSl imesToTeam()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);
            var slime1 = new Slime("Slime 1", ElementType.Fire);
            var slime2 = new Slime("Slime 2", ElementType.Water);
            var slime3 = new Slime("Slime 3", ElementType.Electric);

            expedition.AddSlime(slime1);
            expedition.AddSlime(slime2);
            expedition.AddSlime(slime3);

            Assert.AreEqual(3, expedition.TeamSize);
        }

        [Test]
        public void Expedition_HasMaxTeamSizeLimit()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone, maxTeamSize: 3);

            Assert.AreEqual(3, expedition.MaxTeamSize);
        }

        [Test]
        public void Expedition_CannotExceedMaxTeamSize()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone, maxTeamSize: 2);

            var slime1 = new Slime("Slime 1", ElementType.Fire);
            var slime2 = new Slime("Slime 2", ElementType.Water);
            var slime3 = new Slime("Slime 3", ElementType.Electric);

            expedition.AddSlime(slime1);
            expedition.AddSlime(slime2);

            Assert.Throws<InvalidOperationException>(() => expedition.AddSlime(slime3));
        }

        [Test]
        public void Expedition_DefaultMaxTeamSizeIsFour()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);

            Assert.AreEqual(4, expedition.MaxTeamSize);
        }

        [Test]
        public void Expedition_CannotAddSameSlimeTwice()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Test Slime", ElementType.Fire);

            expedition.AddSlime(slime);

            Assert.Throws<InvalidOperationException>(() => expedition.AddSlime(slime));
        }

        [Test]
        public void Expedition_CanRemoveSlimeFromTeam()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Test Slime", ElementType.Fire);

            expedition.AddSlime(slime);
            expedition.RemoveSlime(slime.ID);

            Assert.AreEqual(0, expedition.TeamSize);
            Assert.IsFalse(expedition.HasSlime(slime.ID));
        }

        [Test]
        public void Expedition_StartExpedition_RemovesSlimesFromLaboratory()
        {
            var lab = new Laboratory("Test Lab");
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);

            var slime1 = new Slime("Slime 1", ElementType.Fire);
            var slime2 = new Slime("Slime 2", ElementType.Water);

            lab.AddSlime(slime1);
            lab.AddSlime(slime2);

            expedition.AddSlime(slime1);
            expedition.AddSlime(slime2);

            expedition.Start(lab);

            // Slimes should be removed from laboratory
            Assert.IsFalse(lab.ContainsSlime(slime1.ID));
            Assert.IsFalse(lab.ContainsSlime(slime2.ID));
            Assert.AreEqual(0, lab.SlimeCount);
        }

        [Test]
        public void Expedition_CannotStartWithEmptyTeam()
        {
            var lab = new Laboratory("Test Lab");
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);

            Assert.Throws<InvalidOperationException>(() => expedition.Start(lab));
        }

        [Test]
        public void Expedition_StatusIsActiveAfterStart()
        {
            var lab = new Laboratory("Test Lab");
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);
            var slime = new Slime("Test Slime", ElementType.Fire);

            lab.AddSlime(slime);
            expedition.AddSlime(slime);

            Assert.AreEqual(ExpeditionStatus.Preparing, expedition.Status);

            expedition.Start(lab);

            Assert.AreEqual(ExpeditionStatus.Active, expedition.Status);
        }

        [Test]
        public void Expedition_CannotAddSlimesAfterStart()
        {
            var lab = new Laboratory("Test Lab");
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);
            var slime1 = new Slime("Slime 1", ElementType.Fire);
            var slime2 = new Slime("Slime 2", ElementType.Water);

            lab.AddSlime(slime1);
            lab.AddSlime(slime2);
            expedition.AddSlime(slime1);

            expedition.Start(lab);

            Assert.Throws<InvalidOperationException>(() => expedition.AddSlime(slime2));
        }

        [Test]
        public void Expedition_CanGetAllTeamMembers()
        {
            var zone = new Zone("Test Zone", 1);
            var expedition = new Expedition(zone);
            var slime1 = new Slime("Slime 1", ElementType.Fire);
            var slime2 = new Slime("Slime 2", ElementType.Water);

            expedition.AddSlime(slime1);
            expedition.AddSlime(slime2);

            var team = expedition.GetTeam();

            Assert.AreEqual(2, team.Count);
            Assert.Contains(slime1, team);
            Assert.Contains(slime2, team);
        }

        [Test]
        public void Expedition_CanValidateSlimesMeetZoneRequirements()
        {
            var zone = new Zone("Advanced Zone", 5);
            var requirement = new ZoneRequirement(minLevel: 10);
            zone.SetRequirement(requirement);

            var expedition = new Expedition(zone);
            var qualifiedSlime = new Slime("Strong Slime", ElementType.Fire);
            qualifiedSlime.SetLevel(12);

            var unqualifiedSlime = new Slime("Weak Slime", ElementType.Water);
            unqualifiedSlime.SetLevel(5);

            // Should be able to add qualified slime
            expedition.AddSlime(qualifiedSlime);

            // Should throw when adding unqualified slime
            Assert.Throws<InvalidOperationException>(() => expedition.AddSlime(unqualifiedSlime));
        }
    }
}
