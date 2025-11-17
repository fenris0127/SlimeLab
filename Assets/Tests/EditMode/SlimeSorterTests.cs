using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class SlimeSorterTests
    {
        [Test]
        public void SlimeSorter_CanBeCreated()
        {
            var sorter = new SlimeSorter();

            Assert.IsNotNull(sorter);
        }

        [Test]
        public void SlimeSorter_CanSortSlimesByElement()
        {
            var sorter = new SlimeSorter();
            var slimes = new List<Slime>
            {
                new Slime("Fire 1", ElementType.Fire),
                new Slime("Water 1", ElementType.Water),
                new Slime("Fire 2", ElementType.Fire),
                new Slime("Electric 1", ElementType.Electric),
                new Slime("Neutral 1", ElementType.Neutral)
            };

            var sorted = sorter.SortByElement(slimes);

            Assert.AreEqual(4, sorted.Count); // 4 element types
            Assert.IsTrue(sorted.ContainsKey(ElementType.Fire));
            Assert.IsTrue(sorted.ContainsKey(ElementType.Water));
            Assert.IsTrue(sorted.ContainsKey(ElementType.Electric));
            Assert.IsTrue(sorted.ContainsKey(ElementType.Neutral));
        }

        [Test]
        public void SlimeSorter_GroupsSlimesByElement()
        {
            var sorter = new SlimeSorter();
            var slimes = new List<Slime>
            {
                new Slime("Fire 1", ElementType.Fire),
                new Slime("Fire 2", ElementType.Fire),
                new Slime("Water 1", ElementType.Water)
            };

            var sorted = sorter.SortByElement(slimes);

            Assert.AreEqual(2, sorted[ElementType.Fire].Count);
            Assert.AreEqual(1, sorted[ElementType.Water].Count);
        }

        [Test]
        public void SlimeSorter_CanSortByLevel()
        {
            var sorter = new SlimeSorter();
            var slimes = new List<Slime>
            {
                new Slime("Low", ElementType.Fire),
                new Slime("Medium", ElementType.Fire),
                new Slime("High", ElementType.Fire)
            };

            slimes[0].SetLevel(5);
            slimes[1].SetLevel(10);
            slimes[2].SetLevel(20);

            var sorted = sorter.SortByLevel(slimes);

            Assert.AreEqual(slimes[2], sorted[0]); // Highest level first
            Assert.AreEqual(slimes[1], sorted[1]);
            Assert.AreEqual(slimes[0], sorted[2]);
        }

        [Test]
        public void SlimeSorter_CanSortByName()
        {
            var sorter = new SlimeSorter();
            var slimes = new List<Slime>
            {
                new Slime("Charlie", ElementType.Fire),
                new Slime("Alice", ElementType.Water),
                new Slime("Bob", ElementType.Electric)
            };

            var sorted = sorter.SortByName(slimes);

            Assert.AreEqual("Alice", sorted[0].Name);
            Assert.AreEqual("Bob", sorted[1].Name);
            Assert.AreEqual("Charlie", sorted[2].Name);
        }

        [Test]
        public void SortingRule_CanBeCreatedWithElementFilter()
        {
            var rule = new SortingRule(ElementType.Fire);

            Assert.IsNotNull(rule);
            Assert.AreEqual(ElementType.Fire, rule.ElementFilter);
        }

        [Test]
        public void SortingRule_CanFilterSlimesByElement()
        {
            var rule = new SortingRule(ElementType.Fire);
            var slimes = new List<Slime>
            {
                new Slime("Fire 1", ElementType.Fire),
                new Slime("Water 1", ElementType.Water),
                new Slime("Fire 2", ElementType.Fire)
            };

            var filtered = rule.Apply(slimes);

            Assert.AreEqual(2, filtered.Count);
            Assert.IsTrue(filtered.TrueForAll(s => s.Element == ElementType.Fire));
        }

        [Test]
        public void SortingRule_CanFilterByMinLevel()
        {
            var rule = new SortingRule(minLevel: 10);
            var slimes = new List<Slime>
            {
                new Slime("Low", ElementType.Fire),
                new Slime("High", ElementType.Fire)
            };

            slimes[0].SetLevel(5);
            slimes[1].SetLevel(15);

            var filtered = rule.Apply(slimes);

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(slimes[1], filtered[0]);
        }

        [Test]
        public void SortingRule_CanFilterByMaxLevel()
        {
            var rule = new SortingRule(maxLevel: 10);
            var slimes = new List<Slime>
            {
                new Slime("Low", ElementType.Fire),
                new Slime("High", ElementType.Fire)
            };

            slimes[0].SetLevel(5);
            slimes[1].SetLevel(15);

            var filtered = rule.Apply(slimes);

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(slimes[0], filtered[0]);
        }

        [Test]
        public void SortingRule_CanCombineMultipleFilters()
        {
            var rule = new SortingRule(ElementType.Fire, minLevel: 10, maxLevel: 20);
            var slimes = new List<Slime>
            {
                new Slime("Fire Low", ElementType.Fire),
                new Slime("Fire Medium", ElementType.Fire),
                new Slime("Fire High", ElementType.Fire),
                new Slime("Water Medium", ElementType.Water)
            };

            slimes[0].SetLevel(5);
            slimes[1].SetLevel(15);
            slimes[2].SetLevel(25);
            slimes[3].SetLevel(15);

            var filtered = rule.Apply(slimes);

            // Only Fire slime with level between 10-20
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(slimes[1], filtered[0]);
        }

        [Test]
        public void SlimeSorter_CanApplySortingRule()
        {
            var sorter = new SlimeSorter();
            var rule = new SortingRule(ElementType.Fire);
            var slimes = new List<Slime>
            {
                new Slime("Fire 1", ElementType.Fire),
                new Slime("Water 1", ElementType.Water),
                new Slime("Fire 2", ElementType.Fire)
            };

            var filtered = sorter.ApplyRule(slimes, rule);

            Assert.AreEqual(2, filtered.Count);
            Assert.IsTrue(filtered.TrueForAll(s => s.Element == ElementType.Fire));
        }

        [Test]
        public void SlimeSorter_CanApplyMultipleRules()
        {
            var sorter = new SlimeSorter();
            var rules = new List<SortingRule>
            {
                new SortingRule(ElementType.Fire),
                new SortingRule(minLevel: 10)
            };

            var slimes = new List<Slime>
            {
                new Slime("Fire Low", ElementType.Fire),
                new Slime("Fire High", ElementType.Fire),
                new Slime("Water High", ElementType.Water)
            };

            slimes[0].SetLevel(5);
            slimes[1].SetLevel(15);
            slimes[2].SetLevel(15);

            var filtered = sorter.ApplyRules(slimes, rules);

            // Only high-level Fire slimes
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(slimes[1], filtered[0]);
        }

        [Test]
        public void SlimeSorter_CanGetSlimesByElementFromLaboratory()
        {
            var sorter = new SlimeSorter();
            var lab = new Laboratory("Test Lab");

            var fire1 = new Slime("Fire 1", ElementType.Fire);
            var fire2 = new Slime("Fire 2", ElementType.Fire);
            var water = new Slime("Water 1", ElementType.Water);

            lab.AddSlime(fire1);
            lab.AddSlime(fire2);
            lab.AddSlime(water);

            var fireSlimes = sorter.GetSlimesByElement(lab, ElementType.Fire);

            Assert.AreEqual(2, fireSlimes.Count);
            Assert.IsTrue(fireSlimes.Contains(fire1));
            Assert.IsTrue(fireSlimes.Contains(fire2));
        }

        [Test]
        public void SortingRule_CanFilterByMood()
        {
            var rule = new SortingRule(moodFilter: SlimeMood.Happy);
            var slimes = new List<Slime>
            {
                new Slime("Happy", ElementType.Fire),
                new Slime("Sad", ElementType.Fire)
            };

            slimes[1].IncreaseHunger(70); // Make sad

            var filtered = rule.Apply(slimes);

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(SlimeMood.Happy, filtered[0].Mood);
        }

        [Test]
        public void SlimeSorter_EmptyListReturnsEmptyResult()
        {
            var sorter = new SlimeSorter();
            var slimes = new List<Slime>();

            var sorted = sorter.SortByElement(slimes);

            Assert.AreEqual(0, sorted.Count);
        }

        [Test]
        public void SortingRule_NoFiltersReturnsAllSlimes()
        {
            var rule = new SortingRule();
            var slimes = new List<Slime>
            {
                new Slime("Fire 1", ElementType.Fire),
                new Slime("Water 1", ElementType.Water)
            };

            var filtered = rule.Apply(slimes);

            Assert.AreEqual(2, filtered.Count);
        }

        [Test]
        public void SlimeSorter_CanGetTopLevelSlimes()
        {
            var sorter = new SlimeSorter();
            var slimes = new List<Slime>
            {
                new Slime("Low", ElementType.Fire),
                new Slime("Medium", ElementType.Fire),
                new Slime("High", ElementType.Fire)
            };

            slimes[0].SetLevel(5);
            slimes[1].SetLevel(10);
            slimes[2].SetLevel(20);

            var top2 = sorter.GetTopLevelSlimes(slimes, 2);

            Assert.AreEqual(2, top2.Count);
            Assert.AreEqual(slimes[2], top2[0]); // Level 20
            Assert.AreEqual(slimes[1], top2[1]); // Level 10
        }

        [Test]
        public void Laboratory_CanUseSlimeSorterToOrganize()
        {
            var lab = new Laboratory("Test Lab");
            var sorter = new SlimeSorter();

            var fire1 = new Slime("Fire 1", ElementType.Fire);
            var fire2 = new Slime("Fire 2", ElementType.Fire);
            var water = new Slime("Water 1", ElementType.Water);

            lab.AddSlime(fire1);
            lab.AddSlime(fire2);
            lab.AddSlime(water);

            // Get all slimes from lab
            var allSlimes = new List<Slime> { fire1, fire2, water };

            var organized = sorter.SortByElement(allSlimes);

            Assert.AreEqual(2, organized.Keys.Count);
            Assert.AreEqual(2, organized[ElementType.Fire].Count);
            Assert.AreEqual(1, organized[ElementType.Water].Count);
        }
    }
}
