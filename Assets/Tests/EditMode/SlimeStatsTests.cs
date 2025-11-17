using NUnit.Framework;
using SlimeLab.Core;

namespace SlimeLab.Tests
{
    public class SlimeStatsTests
    {
        [Test]
        public void SlimeStats_ShouldHaveHP()
        {
            // Arrange
            int expectedHP = 100;

            // Act
            var stats = new SlimeStats(expectedHP, 10, 5, 8);

            // Assert
            Assert.AreEqual(expectedHP, stats.HP);
        }

        [Test]
        public void SlimeStats_ShouldHaveAttack()
        {
            // Arrange
            int expectedAttack = 15;

            // Act
            var stats = new SlimeStats(100, expectedAttack, 5, 8);

            // Assert
            Assert.AreEqual(expectedAttack, stats.Attack);
        }

        [Test]
        public void SlimeStats_ShouldHaveDefense()
        {
            // Arrange
            int expectedDefense = 10;

            // Act
            var stats = new SlimeStats(100, 15, expectedDefense, 8);

            // Assert
            Assert.AreEqual(expectedDefense, stats.Defense);
        }

        [Test]
        public void SlimeStats_ShouldHaveSpeed()
        {
            // Arrange
            int expectedSpeed = 12;

            // Act
            var stats = new SlimeStats(100, 15, 10, expectedSpeed);

            // Assert
            Assert.AreEqual(expectedSpeed, stats.Speed);
        }
    }
}
