using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class SpecialEvolutionTests
    {
        [Test]
        public void Slime_CanEvolveWithMatchingEnvironment()
        {
            // Fire slime in Volcanic environment
            var slime = new Slime("Fire Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Volcano Stone", ElementType.Fire);

            bool canEvolve = slime.CanEvolveInEnvironment(EnvironmentType.Volcanic, evolutionItem);

            Assert.IsTrue(canEvolve);
        }

        [Test]
        public void Slime_CannotEvolveWithMismatchedEnvironment()
        {
            // Fire slime in Aquatic environment
            var slime = new Slime("Fire Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Water Stone", ElementType.Water);

            bool canEvolve = slime.CanEvolveInEnvironment(EnvironmentType.Aquatic, evolutionItem);

            Assert.IsFalse(canEvolve);
        }

        [Test]
        public void Slime_EvolvesIntoSpecialFormInMatchingEnvironment()
        {
            // Fire slime in Volcanic environment gets special evolution
            var slime = new Slime("Fire Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Volcano Stone", ElementType.Fire);

            slime.EvolveInEnvironment(EnvironmentType.Volcanic, evolutionItem);

            // Should get a special name for environment-based evolution
            Assert.AreEqual("Magma Titan", slime.Name);
        }

        [Test]
        public void WaterSlime_EvolvesIntoSpecialFormInAquaticEnvironment()
        {
            var slime = new Slime("Water Slime", ElementType.Water);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Ocean Pearl", ElementType.Water);

            slime.EvolveInEnvironment(EnvironmentType.Aquatic, evolutionItem);

            Assert.AreEqual("Oceanic Guardian", slime.Name);
        }

        [Test]
        public void ElectricSlime_EvolvesIntoSpecialFormInStormEnvironment()
        {
            var slime = new Slime("Electric Slime", ElementType.Electric);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Thunder Crystal", ElementType.Electric);

            slime.EvolveInEnvironment(EnvironmentType.Storm, evolutionItem);

            Assert.AreEqual("Tempest Sovereign", slime.Name);
        }

        [Test]
        public void Slime_CanEvolveAtNight()
        {
            var slime = new Slime("Moon Slime", ElementType.Neutral);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Moon Stone", ElementType.Neutral);

            // Night time (22:00)
            var nightTime = new DateTime(2025, 1, 1, 22, 0, 0);

            bool canEvolve = slime.CanEvolveAtTime(evolutionItem, nightTime);

            Assert.IsTrue(canEvolve);
        }

        [Test]
        public void Slime_CannotEvolveDuringDay_WhenNightRequired()
        {
            var slime = new Slime("Moon Slime", ElementType.Neutral);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Moon Stone", ElementType.Neutral);

            // Day time (12:00)
            var dayTime = new DateTime(2025, 1, 1, 12, 0, 0);

            bool canEvolve = slime.CanEvolveAtTime(evolutionItem, dayTime);

            Assert.IsFalse(canEvolve);
        }

        [Test]
        public void Slime_EvolvesIntoNocturnalFormAtNight()
        {
            var slime = new Slime("Moon Slime", ElementType.Neutral);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Moon Stone", ElementType.Neutral);
            var nightTime = new DateTime(2025, 1, 1, 22, 0, 0);

            slime.EvolveAtTime(evolutionItem, nightTime);

            Assert.AreEqual("Lunar Eclipse", slime.Name);
        }

        [Test]
        public void Slime_CanEvolveAtDay()
        {
            var slime = new Slime("Sun Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Sun Stone", ElementType.Fire);

            // Day time (14:00)
            var dayTime = new DateTime(2025, 1, 1, 14, 0, 0);

            bool canEvolve = slime.CanEvolveAtTime(evolutionItem, dayTime);

            Assert.IsTrue(canEvolve);
        }

        [Test]
        public void Slime_EvolvesIntoDiurnalFormAtDay()
        {
            var slime = new Slime("Sun Slime", ElementType.Fire);
            slime.SetLevel(10);
            var evolutionItem = new EvolutionItem("Sun Stone", ElementType.Fire);
            var dayTime = new DateTime(2025, 1, 1, 14, 0, 0);

            slime.EvolveAtTime(evolutionItem, dayTime);

            Assert.AreEqual("Solar Flare", slime.Name);
        }

        [Test]
        public void Slime_CanEvolveWithHighAffinity()
        {
            var slime = new Slime("Loyal Slime", ElementType.Neutral);
            slime.SetLevel(10);
            slime.SetAffinity(80); // High affinity
            var evolutionItem = new EvolutionItem("Friendship Stone", ElementType.Neutral);

            bool canEvolve = slime.CanEvolveWithAffinity(evolutionItem);

            Assert.IsTrue(canEvolve);
        }

        [Test]
        public void Slime_CannotEvolveWithLowAffinity()
        {
            var slime = new Slime("New Slime", ElementType.Neutral);
            slime.SetLevel(10);
            slime.SetAffinity(30); // Low affinity
            var evolutionItem = new EvolutionItem("Friendship Stone", ElementType.Neutral);

            bool canEvolve = slime.CanEvolveWithAffinity(evolutionItem);

            Assert.IsFalse(canEvolve);
        }

        [Test]
        public void Slime_EvolvesIntoAffinityFormWithHighAffinity()
        {
            var slime = new Slime("Loyal Slime", ElementType.Neutral);
            slime.SetLevel(10);
            slime.SetAffinity(90);
            var evolutionItem = new EvolutionItem("Friendship Stone", ElementType.Neutral);

            slime.EvolveWithAffinity(evolutionItem);

            Assert.AreEqual("Eternal Bond", slime.Name);
        }

        [Test]
        public void Slime_AffinityEvolutionBoostsStatsMore()
        {
            var slime = new Slime("Loyal Slime", ElementType.Neutral);
            slime.SetLevel(10);
            slime.SetAffinity(95);
            var evolutionItem = new EvolutionItem("Friendship Stone", ElementType.Neutral);

            int initialHP = slime.Stats.HP;
            int initialAttack = slime.Stats.Attack;

            slime.EvolveWithAffinity(evolutionItem);

            // Affinity evolution should give bigger stat boosts
            Assert.Greater(slime.Stats.HP - initialHP, 50); // More than normal evolution
            Assert.Greater(slime.Stats.Attack - initialAttack, 20);
        }

        [Test]
        public void Slime_AffinityIncreasesOverTime()
        {
            var slime = new Slime("Test Slime", ElementType.Neutral);

            int initialAffinity = slime.Affinity;
            slime.IncreaseAffinity(10);

            Assert.AreEqual(initialAffinity + 10, slime.Affinity);
        }

        [Test]
        public void Slime_AffinityHasMaximumCap()
        {
            var slime = new Slime("Test Slime", ElementType.Neutral);
            slime.SetAffinity(95);

            slime.IncreaseAffinity(20); // Try to go over 100

            Assert.AreEqual(100, slime.Affinity); // Capped at 100
        }
    }
}
