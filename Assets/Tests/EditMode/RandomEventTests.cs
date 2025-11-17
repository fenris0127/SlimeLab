using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class RandomEventTests
    {
        [Test]
        public void RandomEvent_CanBeCreated()
        {
            var randomEvent = new RandomEvent("RE001", "Strange Visitor", "A mysterious figure appears at your lab.");

            Assert.IsNotNull(randomEvent);
            Assert.AreEqual("RE001", randomEvent.ID);
            Assert.AreEqual("Strange Visitor", randomEvent.Title);
            Assert.AreEqual("A mysterious figure appears at your lab.", randomEvent.Description);
        }

        [Test]
        public void RandomEvent_HasOccurrenceProbability()
        {
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description", probability: 0.25f);

            Assert.AreEqual(0.25f, randomEvent.Probability);
        }

        [Test]
        public void RandomEvent_CanHaveMultipleChoices()
        {
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description");
            var choice1 = new EventChoice("Accept", "Accept the offer");
            var choice2 = new EventChoice("Decline", "Decline the offer");

            randomEvent.AddChoice(choice1);
            randomEvent.AddChoice(choice2);

            Assert.AreEqual(2, randomEvent.Choices.Count);
            Assert.Contains(choice1, randomEvent.Choices);
            Assert.Contains(choice2, randomEvent.Choices);
        }

        [Test]
        public void EventChoice_CanBeCreated()
        {
            var choice = new EventChoice("Accept", "Accept the offer");

            Assert.IsNotNull(choice);
            Assert.AreEqual("Accept", choice.ID);
            Assert.AreEqual("Accept the offer", choice.Description);
        }

        [Test]
        public void EventChoice_CanHaveOutcome()
        {
            var choice = new EventChoice("Accept", "Accept the offer");
            var outcome = new EventOutcome();
            outcome.AddResourceReward(ResourceType.Research, 100);

            choice.SetOutcome(outcome);

            Assert.AreEqual(outcome, choice.Outcome);
        }

        [Test]
        public void EventOutcome_CanProvideResourceReward()
        {
            var outcome = new EventOutcome();
            outcome.AddResourceReward(ResourceType.Research, 100);
            outcome.AddResourceReward(ResourceType.Energy, 50);

            var rewards = outcome.GetResourceRewards();

            Assert.AreEqual(2, rewards.Count);
            Assert.AreEqual(100, rewards[ResourceType.Research]);
            Assert.AreEqual(50, rewards[ResourceType.Energy]);
        }

        [Test]
        public void EventOutcome_CanApplyToInventory()
        {
            var outcome = new EventOutcome();
            outcome.AddResourceReward(ResourceType.Research, 100);
            outcome.AddResourceReward(ResourceType.Energy, 50);

            var inventory = new ResourceInventory();
            outcome.ApplyToInventory(inventory);

            Assert.AreEqual(100, inventory.GetResourceAmount(ResourceType.Research));
            Assert.AreEqual(50, inventory.GetResourceAmount(ResourceType.Energy));
        }

        [Test]
        public void EventScheduler_CanBeCreated()
        {
            var scheduler = new EventScheduler();

            Assert.IsNotNull(scheduler);
        }

        [Test]
        public void EventScheduler_CanAddEvent()
        {
            var scheduler = new EventScheduler();
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description", probability: 0.5f);

            scheduler.AddEvent(randomEvent);

            Assert.AreEqual(1, scheduler.EventCount);
        }

        [Test]
        public void EventScheduler_CheckEventReturnsProbabilisticResult()
        {
            var scheduler = new EventScheduler();
            var certainEvent = new RandomEvent("RE001", "Certain Event", "Always triggers", probability: 1.0f);
            var impossibleEvent = new RandomEvent("RE002", "Impossible Event", "Never triggers", probability: 0.0f);

            scheduler.AddEvent(certainEvent);
            scheduler.AddEvent(impossibleEvent);

            // Check 100 times - certain event should trigger at least once, impossible never
            bool certainTriggered = false;
            bool impossibleTriggered = false;

            for (int i = 0; i < 100; i++)
            {
                var triggeredEvent = scheduler.CheckForEvent();
                if (triggeredEvent != null)
                {
                    if (triggeredEvent.ID == "RE001") certainTriggered = true;
                    if (triggeredEvent.ID == "RE002") impossibleTriggered = true;
                }
            }

            Assert.IsTrue(certainTriggered);
            Assert.IsFalse(impossibleTriggered);
        }

        [Test]
        public void EventScheduler_ReturnsNullWhenNoEventTriggered()
        {
            var scheduler = new EventScheduler();
            var impossibleEvent = new RandomEvent("RE001", "Impossible Event", "Never triggers", probability: 0.0f);

            scheduler.AddEvent(impossibleEvent);

            var triggeredEvent = scheduler.CheckForEvent();

            Assert.IsNull(triggeredEvent);
        }

        [Test]
        public void RandomEvent_CanBeTriggered()
        {
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description");

            Assert.IsFalse(randomEvent.IsTriggered);

            randomEvent.Trigger();

            Assert.IsTrue(randomEvent.IsTriggered);
        }

        [Test]
        public void RandomEvent_CanBeResolved()
        {
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description");
            var choice = new EventChoice("Accept", "Accept the offer");

            randomEvent.AddChoice(choice);
            randomEvent.Trigger();

            Assert.IsFalse(randomEvent.IsResolved);

            randomEvent.Resolve(choice);

            Assert.IsTrue(randomEvent.IsResolved);
        }

        [Test]
        public void RandomEvent_CannotResolveWithoutTriggering()
        {
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description");
            var choice = new EventChoice("Accept", "Accept the offer");

            randomEvent.AddChoice(choice);

            Assert.Throws<System.InvalidOperationException>(() => randomEvent.Resolve(choice));
        }

        [Test]
        public void RandomEvent_CannotResolveWithInvalidChoice()
        {
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description");
            var validChoice = new EventChoice("Accept", "Accept the offer");
            var invalidChoice = new EventChoice("Invalid", "Not an option");

            randomEvent.AddChoice(validChoice);
            randomEvent.Trigger();

            Assert.Throws<System.InvalidOperationException>(() => randomEvent.Resolve(invalidChoice));
        }

        [Test]
        public void EventScheduler_CanRemoveTriggeredEvents()
        {
            var scheduler = new EventScheduler();
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description");

            scheduler.AddEvent(randomEvent);
            Assert.AreEqual(1, scheduler.EventCount);

            randomEvent.Trigger();
            scheduler.RemoveTriggeredEvents();

            Assert.AreEqual(0, scheduler.EventCount);
        }

        [Test]
        public void EventOutcome_CanHaveDescription()
        {
            var outcome = new EventOutcome("You gained valuable resources!");

            Assert.AreEqual("You gained valuable resources!", outcome.Description);
        }

        [Test]
        public void EventScheduler_CanSetSeedForDeterministicTesting()
        {
            var scheduler1 = new EventScheduler(seed: 12345);
            var scheduler2 = new EventScheduler(seed: 12345);
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description", probability: 0.5f);

            scheduler1.AddEvent(randomEvent);
            scheduler2.AddEvent(randomEvent);

            // With same seed, both schedulers should behave identically
            var result1 = scheduler1.CheckForEvent();
            var result2 = scheduler2.CheckForEvent();

            // Both should either trigger or not trigger (same behavior)
            Assert.AreEqual(result1 != null, result2 != null);
        }

        [Test]
        public void EventScheduler_CanGetAllEvents()
        {
            var scheduler = new EventScheduler();
            var event1 = new RandomEvent("RE001", "Event 1", "Description 1");
            var event2 = new RandomEvent("RE002", "Event 2", "Description 2");

            scheduler.AddEvent(event1);
            scheduler.AddEvent(event2);

            var allEvents = scheduler.GetAllEvents();

            Assert.AreEqual(2, allEvents.Count);
            Assert.Contains(event1, allEvents);
            Assert.Contains(event2, allEvents);
        }

        [Test]
        public void EventOutcome_CanProvideNegativeResourceChange()
        {
            var outcome = new EventOutcome("You lost some resources!");
            outcome.AddResourceReward(ResourceType.Energy, -30); // Negative = cost

            var inventory = new ResourceInventory();
            inventory.AddResource(new Resource(ResourceType.Energy, 100));

            outcome.ApplyToInventory(inventory);

            Assert.AreEqual(70, inventory.GetResourceAmount(ResourceType.Energy));
        }

        [Test]
        public void RandomEvent_CanCheckIfChoiceIsValid()
        {
            var randomEvent = new RandomEvent("RE001", "Test Event", "Description");
            var validChoice = new EventChoice("Accept", "Accept the offer");
            var invalidChoice = new EventChoice("Invalid", "Not an option");

            randomEvent.AddChoice(validChoice);

            Assert.IsTrue(randomEvent.HasChoice(validChoice));
            Assert.IsFalse(randomEvent.HasChoice(invalidChoice));
        }

        [Test]
        public void EventScheduler_CanClearAllEvents()
        {
            var scheduler = new EventScheduler();
            scheduler.AddEvent(new RandomEvent("RE001", "Event 1", "Description 1"));
            scheduler.AddEvent(new RandomEvent("RE002", "Event 2", "Description 2"));

            Assert.AreEqual(2, scheduler.EventCount);

            scheduler.ClearAllEvents();

            Assert.AreEqual(0, scheduler.EventCount);
        }
    }
}
