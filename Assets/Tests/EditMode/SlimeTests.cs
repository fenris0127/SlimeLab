using NUnit.Framework;
using SlimeLab.Core;

namespace SlimeLab.Tests
{
    public class SlimeTests
    {
        [Test]
        public void Slime_ShouldHaveID()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.IsNotNull(slime.ID);
            Assert.IsNotEmpty(slime.ID);
        }

        [Test]
        public void Slime_ShouldHaveName()
        {
            // Arrange
            string expectedName = "Slimey";

            // Act
            var slime = new Slime(expectedName);

            // Assert
            Assert.IsNotNull(slime.Name);
            Assert.AreEqual(expectedName, slime.Name);
        }

        [Test]
        public void Slime_ShouldHaveElementType()
        {
            // Arrange & Act
            var fireSlime = new Slime("Fire Slime", ElementType.Fire);
            var waterSlime = new Slime("Water Slime", ElementType.Water);
            var electricSlime = new Slime("Electric Slime", ElementType.Electric);
            var neutralSlime = new Slime("Neutral Slime", ElementType.Neutral);

            // Assert
            Assert.AreEqual(ElementType.Fire, fireSlime.Element);
            Assert.AreEqual(ElementType.Water, waterSlime.Element);
            Assert.AreEqual(ElementType.Electric, electricSlime.Element);
            Assert.AreEqual(ElementType.Neutral, neutralSlime.Element);
        }

        [Test]
        public void Slime_ShouldHaveDefaultLevelOfOne()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.AreEqual(1, slime.Level);
        }

        [Test]
        public void Slime_ShouldHaveDefaultExperienceOfZero()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.AreEqual(0, slime.Experience);
        }

        [Test]
        public void Slime_ShouldHaveStats()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.IsNotNull(slime.Stats);
            Assert.IsInstanceOf<SlimeStats>(slime.Stats);
            Assert.Greater(slime.Stats.HP, 0);
            Assert.GreaterOrEqual(slime.Stats.Attack, 0);
            Assert.GreaterOrEqual(slime.Stats.Defense, 0);
            Assert.Greater(slime.Stats.Speed, 0);
        }

        [Test]
        public void Slime_ShouldHaveHungerLevel()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.GreaterOrEqual(slime.Hunger, 0);
            Assert.LessOrEqual(slime.Hunger, 100);
        }

        [Test]
        public void Slime_ShouldStartWithZeroHunger()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.AreEqual(0, slime.Hunger);
        }

        [Test]
        public void Slime_HungerCanIncrease()
        {
            // Arrange
            var slime = new Slime();
            int initialHunger = slime.Hunger;

            // Act
            slime.IncreaseHunger(10);

            // Assert
            Assert.AreEqual(initialHunger + 10, slime.Hunger);
        }

        [Test]
        public void Slime_HungerCannotExceed100()
        {
            // Arrange
            var slime = new Slime();

            // Act
            slime.IncreaseHunger(150);

            // Assert
            Assert.AreEqual(100, slime.Hunger);
        }

        [Test]
        public void Slime_CanBeFed()
        {
            // Arrange
            var slime = new Slime();
            slime.IncreaseHunger(50);

            // Act
            slime.Feed(30);

            // Assert
            Assert.AreEqual(20, slime.Hunger);
        }

        [Test]
        public void Slime_FeedingDecreasesHungerAndIncreasesExperience()
        {
            // Arrange
            var slime = new Slime();
            slime.IncreaseHunger(40);
            int initialExp = slime.Experience;

            // Act
            slime.Feed(20);

            // Assert
            Assert.AreEqual(20, slime.Hunger);
            Assert.Greater(slime.Experience, initialExp);
        }

        [Test]
        public void Slime_HungerCannotGoBelowZero()
        {
            // Arrange
            var slime = new Slime();
            slime.IncreaseHunger(10);

            // Act
            slime.Feed(20);

            // Assert
            Assert.AreEqual(0, slime.Hunger);
        }

        [Test]
        public void Slime_ShouldHaveMood()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.AreEqual(SlimeMood.Happy, slime.Mood);
        }

        [Test]
        public void Slime_BecomesSadWhenVeryHungry()
        {
            // Arrange
            var slime = new Slime();

            // Act
            slime.IncreaseHunger(80);

            // Assert
            Assert.AreEqual(SlimeMood.Sad, slime.Mood);
        }

        [Test]
        public void Slime_BecomesUnhappyWhenExtremelyHungry()
        {
            // Arrange
            var slime = new Slime();

            // Act
            slime.IncreaseHunger(100);

            // Assert
            Assert.AreEqual(SlimeMood.Unhappy, slime.Mood);
        }

        [Test]
        public void Slime_BecomesHappyAfterBeingFed()
        {
            // Arrange
            var slime = new Slime();
            slime.IncreaseHunger(90);

            // Act
            slime.Feed(70);

            // Assert
            Assert.AreEqual(SlimeMood.Happy, slime.Mood);
        }

        [Test]
        public void Slime_ShouldHaveGenesCollection()
        {
            // Arrange & Act
            var slime = new Slime();

            // Assert
            Assert.IsNotNull(slime.Genes);
        }

        [Test]
        public void Slime_CanAddGenes()
        {
            // Arrange
            var slime = new Slime();
            var gene1 = new Gene("Fire Affinity", GeneType.Dominant);
            var gene2 = new Gene("Speed Boost", GeneType.Recessive);

            // Act
            slime.AddGene(gene1);
            slime.AddGene(gene2);

            // Assert
            Assert.AreEqual(2, slime.Genes.Count);
        }

        [Test]
        public void Slime_CanRetrieveGenesById()
        {
            // Arrange
            var slime = new Slime();
            var gene = new Gene("Fire Affinity", GeneType.Dominant);
            slime.AddGene(gene);

            // Act
            var retrievedGene = slime.GetGene(gene.ID);

            // Assert
            Assert.IsNotNull(retrievedGene);
            Assert.AreEqual(gene.ID, retrievedGene.ID);
            Assert.AreEqual(gene.Name, retrievedGene.Name);
        }

        [Test]
        public void Slime_CanCheckIfGeneExists()
        {
            // Arrange
            var slime = new Slime();
            var gene = new Gene("Water Affinity");
            slime.AddGene(gene);

            // Act & Assert
            Assert.IsTrue(slime.HasGene(gene.ID));
            Assert.IsFalse(slime.HasGene("non-existent-id"));
        }
    }
}
