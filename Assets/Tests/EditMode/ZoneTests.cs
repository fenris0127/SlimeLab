using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class ZoneTests
    {
        [Test]
        public void Zone_ShouldHaveIDNameAndDifficulty()
        {
            var zone = new Zone("Forest Zone", 1);

            Assert.IsNotNull(zone.ID);
            Assert.AreEqual("Forest Zone", zone.Name);
            Assert.AreEqual(1, zone.Difficulty);
        }

        [Test]
        public void Zone_IDShouldBeUnique()
        {
            var zone1 = new Zone("Zone 1", 1);
            var zone2 = new Zone("Zone 2", 1);

            Assert.AreNotEqual(zone1.ID, zone2.ID);
        }

        [Test]
        public void Zone_ShouldHaveGridMap()
        {
            var zone = new Zone("Test Zone", 1);
            var gridMap = new GridMap(10, 10);
            zone.SetGridMap(gridMap);

            Assert.IsNotNull(zone.GridMap);
            Assert.AreEqual(10, zone.GridMap.Width);
            Assert.AreEqual(10, zone.GridMap.Height);
        }

        [Test]
        public void GridMap_ShouldHaveWidthAndHeight()
        {
            var gridMap = new GridMap(15, 20);

            Assert.AreEqual(15, gridMap.Width);
            Assert.AreEqual(20, gridMap.Height);
        }

        [Test]
        public void GridMap_ShouldInitializeCells()
        {
            var gridMap = new GridMap(5, 5);

            // Should be able to access cells
            var cell = gridMap.GetCell(2, 3);
            Assert.IsNotNull(cell);
        }

        [Test]
        public void GridMap_ShouldReturnNullForOutOfBoundsCells()
        {
            var gridMap = new GridMap(5, 5);

            var cell = gridMap.GetCell(10, 10);
            Assert.IsNull(cell);
        }

        [Test]
        public void GridCell_ShouldHaveCoordinates()
        {
            var gridMap = new GridMap(5, 5);
            var cell = gridMap.GetCell(2, 3);

            Assert.AreEqual(2, cell.X);
            Assert.AreEqual(3, cell.Y);
        }

        [Test]
        public void GridCell_ShouldHaveTerrainType()
        {
            var gridMap = new GridMap(5, 5);
            var cell = gridMap.GetCell(1, 1);

            // Default terrain should be Normal
            Assert.AreEqual(TerrainType.Normal, cell.TerrainType);
        }

        [Test]
        public void GridCell_CanSetTerrainType()
        {
            var gridMap = new GridMap(5, 5);
            var cell = gridMap.GetCell(2, 2);

            cell.SetTerrainType(TerrainType.Water);

            Assert.AreEqual(TerrainType.Water, cell.TerrainType);
        }

        [Test]
        public void Zone_ShouldHaveEntryRequirement()
        {
            var zone = new Zone("Advanced Zone", 5);
            var requirement = new ZoneRequirement(minLevel: 10);
            zone.SetRequirement(requirement);

            Assert.IsNotNull(zone.Requirement);
            Assert.AreEqual(10, zone.Requirement.MinLevel);
        }

        [Test]
        public void ZoneRequirement_ShouldHaveMinLevel()
        {
            var requirement = new ZoneRequirement(minLevel: 15);

            Assert.AreEqual(15, requirement.MinLevel);
        }

        [Test]
        public void ZoneRequirement_CanCheckIfSlimeMeetsRequirement()
        {
            var requirement = new ZoneRequirement(minLevel: 10);
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(12);

            bool meetsRequirement = requirement.IsMet(slime);

            Assert.IsTrue(meetsRequirement);
        }

        [Test]
        public void ZoneRequirement_SlimeBelowMinLevelShouldNotMeetRequirement()
        {
            var requirement = new ZoneRequirement(minLevel: 10);
            var slime = new Slime("Test Slime", ElementType.Fire);
            slime.SetLevel(5);

            bool meetsRequirement = requirement.IsMet(slime);

            Assert.IsFalse(meetsRequirement);
        }

        [Test]
        public void ZoneRequirement_CanRequireSpecificElement()
        {
            var requirement = new ZoneRequirement(minLevel: 1, requiredElement: ElementType.Fire);
            var fireSlime = new Slime("Fire Slime", ElementType.Fire);
            var waterSlime = new Slime("Water Slime", ElementType.Water);

            Assert.IsTrue(requirement.IsMet(fireSlime));
            Assert.IsFalse(requirement.IsMet(waterSlime));
        }

        [Test]
        public void Zone_CanCheckIfSlimeCanEnter()
        {
            var zone = new Zone("Fire Zone", 3);
            var requirement = new ZoneRequirement(minLevel: 10, requiredElement: ElementType.Fire);
            zone.SetRequirement(requirement);

            var qualifiedSlime = new Slime("Fire Slime", ElementType.Fire);
            qualifiedSlime.SetLevel(12);

            var unqualifiedSlime = new Slime("Water Slime", ElementType.Water);
            unqualifiedSlime.SetLevel(15);

            Assert.IsTrue(zone.CanEnter(qualifiedSlime));
            Assert.IsFalse(zone.CanEnter(unqualifiedSlime));
        }

        [Test]
        public void Zone_WithNoRequirement_AllowsAnySlime()
        {
            var zone = new Zone("Open Zone", 1);
            var slime = new Slime("Any Slime", ElementType.Neutral);

            Assert.IsTrue(zone.CanEnter(slime));
        }
    }
}
