using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    public class ContainmentUnitTests
    {
        [Test]
        public void ContainmentUnit_CanStoreOneSlime()
        {
            // Arrange
            var unit = new ContainmentUnit();
            var slime = new Slime("Test Slime");

            // Act
            unit.AssignSlime(slime);

            // Assert
            Assert.IsNotNull(unit.AssignedSlime);
            Assert.AreEqual(slime.ID, unit.AssignedSlime.ID);
        }

        [Test]
        public void ContainmentUnit_CanBeEmpty()
        {
            // Arrange & Act
            var unit = new ContainmentUnit();

            // Assert
            Assert.IsNull(unit.AssignedSlime);
            Assert.IsFalse(unit.HasSlime);
        }

        [Test]
        public void ContainmentUnit_CanRemoveSlime()
        {
            // Arrange
            var unit = new ContainmentUnit();
            var slime = new Slime("Test Slime");
            unit.AssignSlime(slime);

            // Act
            var removedSlime = unit.RemoveSlime();

            // Assert
            Assert.IsNotNull(removedSlime);
            Assert.AreEqual(slime.ID, removedSlime.ID);
            Assert.IsFalse(unit.HasSlime);
        }

        [Test]
        public void ContainmentUnit_HasEnvironmentType()
        {
            // Arrange & Act
            var fireUnit = new ContainmentUnit(EnvironmentType.Volcanic);
            var waterUnit = new ContainmentUnit(EnvironmentType.Aquatic);
            var electricUnit = new ContainmentUnit(EnvironmentType.Storm);
            var neutralUnit = new ContainmentUnit(EnvironmentType.Standard);

            // Assert
            Assert.AreEqual(EnvironmentType.Volcanic, fireUnit.EnvironmentType);
            Assert.AreEqual(EnvironmentType.Aquatic, waterUnit.EnvironmentType);
            Assert.AreEqual(EnvironmentType.Storm, electricUnit.EnvironmentType);
            Assert.AreEqual(EnvironmentType.Standard, neutralUnit.EnvironmentType);
        }

        [Test]
        public void ContainmentUnit_MatchingElementGives100PercentEfficiency()
        {
            // Arrange
            var fireUnit = new ContainmentUnit(EnvironmentType.Volcanic);
            var fireSlime = new Slime("Fire Slime", ElementType.Fire);
            fireUnit.AssignSlime(fireSlime);

            // Act
            float efficiency = fireUnit.GetEfficiency();

            // Assert
            Assert.AreEqual(1.0f, efficiency);
        }

        [Test]
        public void ContainmentUnit_MismatchedElementGives70PercentEfficiency()
        {
            // Arrange
            var fireUnit = new ContainmentUnit(EnvironmentType.Volcanic);
            var waterSlime = new Slime("Water Slime", ElementType.Water);
            fireUnit.AssignSlime(waterSlime);

            // Act
            float efficiency = fireUnit.GetEfficiency();

            // Assert
            Assert.AreEqual(0.7f, efficiency);
        }

        [Test]
        public void ContainmentUnit_NeutralElementAlwaysGives85PercentEfficiency()
        {
            // Arrange
            var fireUnit = new ContainmentUnit(EnvironmentType.Volcanic);
            var neutralSlime = new Slime("Neutral Slime", ElementType.Neutral);
            fireUnit.AssignSlime(neutralSlime);

            // Act
            float efficiency = fireUnit.GetEfficiency();

            // Assert
            Assert.AreEqual(0.85f, efficiency);
        }

        [Test]
        public void ContainmentUnit_StandardEnvironmentGives90PercentEfficiencyForAll()
        {
            // Arrange
            var standardUnit = new ContainmentUnit(EnvironmentType.Standard);
            var fireSlime = new Slime("Fire Slime", ElementType.Fire);
            standardUnit.AssignSlime(fireSlime);

            // Act
            float efficiency = standardUnit.GetEfficiency();

            // Assert
            Assert.AreEqual(0.9f, efficiency);
        }

        [Test]
        public void ContainmentUnit_EmptyUnitReturns100PercentEfficiency()
        {
            // Arrange
            var unit = new ContainmentUnit(EnvironmentType.Volcanic);

            // Act
            float efficiency = unit.GetEfficiency();

            // Assert
            Assert.AreEqual(1.0f, efficiency);
        }
    }
}
