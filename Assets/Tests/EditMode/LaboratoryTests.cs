using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System;

namespace SlimeLab.Tests
{
    public class LaboratoryTests
    {
        [Test]
        public void Laboratory_ShouldHaveIDAndName()
        {
            // Arrange
            string expectedName = "Research Lab Alpha";

            // Act
            var lab = new Laboratory(expectedName);

            // Assert
            Assert.IsNotNull(lab.ID);
            Assert.IsNotEmpty(lab.ID);
            Assert.AreEqual(expectedName, lab.Name);
        }

        [Test]
        public void Laboratory_ShouldHaveMaxCapacity()
        {
            // Arrange
            int expectedCapacity = 10;

            // Act
            var lab = new Laboratory("Test Lab", expectedCapacity);

            // Assert
            Assert.AreEqual(expectedCapacity, lab.MaxCapacity);
        }

        [Test]
        public void Laboratory_CanAddSlimes()
        {
            // Arrange
            var lab = new Laboratory("Test Lab", 5);
            var slime = new Slime("Test Slime");

            // Act
            lab.AddSlime(slime);

            // Assert
            Assert.AreEqual(1, lab.SlimeCount);
            Assert.IsTrue(lab.ContainsSlime(slime.ID));
        }

        [Test]
        public void Laboratory_CanStoreMultipleSlimes()
        {
            // Arrange
            var lab = new Laboratory("Test Lab", 5);
            var slime1 = new Slime("Slime 1");
            var slime2 = new Slime("Slime 2");
            var slime3 = new Slime("Slime 3");

            // Act
            lab.AddSlime(slime1);
            lab.AddSlime(slime2);
            lab.AddSlime(slime3);

            // Assert
            Assert.AreEqual(3, lab.SlimeCount);
        }

        [Test]
        public void Laboratory_ThrowsExceptionWhenCapacityExceeded()
        {
            // Arrange
            var lab = new Laboratory("Small Lab", 2);
            var slime1 = new Slime("Slime 1");
            var slime2 = new Slime("Slime 2");
            var slime3 = new Slime("Slime 3");

            lab.AddSlime(slime1);
            lab.AddSlime(slime2);

            // Act & Assert
            Assert.Throws<CapacityExceededException>(() =>
            {
                lab.AddSlime(slime3);
            });
        }

        [Test]
        public void Laboratory_CanRemoveSlimes()
        {
            // Arrange
            var lab = new Laboratory("Test Lab", 5);
            var slime = new Slime("Test Slime");
            lab.AddSlime(slime);

            // Act
            lab.RemoveSlime(slime.ID);

            // Assert
            Assert.AreEqual(0, lab.SlimeCount);
            Assert.IsFalse(lab.ContainsSlime(slime.ID));
        }

        [Test]
        public void Laboratory_CanHaveMultipleContainmentUnits()
        {
            // Arrange
            var lab = new Laboratory("Test Lab", 10);

            // Act
            lab.AddContainmentUnit(new ContainmentUnit(EnvironmentType.Volcanic));
            lab.AddContainmentUnit(new ContainmentUnit(EnvironmentType.Aquatic));
            lab.AddContainmentUnit(new ContainmentUnit(EnvironmentType.Storm));

            // Assert
            Assert.AreEqual(3, lab.ContainmentUnitCount);
        }

        [Test]
        public void Laboratory_CanGetContainmentUnitByIndex()
        {
            // Arrange
            var lab = new Laboratory("Test Lab", 10);
            var volcanicUnit = new ContainmentUnit(EnvironmentType.Volcanic);
            var aquaticUnit = new ContainmentUnit(EnvironmentType.Aquatic);

            lab.AddContainmentUnit(volcanicUnit);
            lab.AddContainmentUnit(aquaticUnit);

            // Act
            var firstUnit = lab.GetContainmentUnit(0);
            var secondUnit = lab.GetContainmentUnit(1);

            // Assert
            Assert.IsNotNull(firstUnit);
            Assert.IsNotNull(secondUnit);
            Assert.AreEqual(EnvironmentType.Volcanic, firstUnit.EnvironmentType);
            Assert.AreEqual(EnvironmentType.Aquatic, secondUnit.EnvironmentType);
        }

        [Test]
        public void Laboratory_CanRemoveContainmentUnit()
        {
            // Arrange
            var lab = new Laboratory("Test Lab", 10);
            var unit = new ContainmentUnit(EnvironmentType.Volcanic);
            lab.AddContainmentUnit(unit);

            // Act
            lab.RemoveContainmentUnit(0);

            // Assert
            Assert.AreEqual(0, lab.ContainmentUnitCount);
        }

        [Test]
        public void Laboratory_CanGetAllContainmentUnits()
        {
            // Arrange
            var lab = new Laboratory("Test Lab", 10);
            lab.AddContainmentUnit(new ContainmentUnit(EnvironmentType.Volcanic));
            lab.AddContainmentUnit(new ContainmentUnit(EnvironmentType.Aquatic));

            // Act
            var units = lab.GetAllContainmentUnits();

            // Assert
            Assert.AreEqual(2, units.Count);
        }
    }
}
