using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class StoryEventTests
    {
        [Test]
        public void StoryProgress_CanBeCreated()
        {
            var progress = new StoryProgress();

            Assert.IsNotNull(progress);
        }

        [Test]
        public void StoryProgress_CanSetMilestone()
        {
            var progress = new StoryProgress();

            progress.SetMilestone("FirstSlimeBred");

            Assert.IsTrue(progress.HasMilestone("FirstSlimeBred"));
        }

        [Test]
        public void StoryProgress_ReturnsFalseForUnsetMilestone()
        {
            var progress = new StoryProgress();

            Assert.IsFalse(progress.HasMilestone("NonExistentMilestone"));
        }

        [Test]
        public void StoryProgress_CanSetProgressValue()
        {
            var progress = new StoryProgress();

            progress.SetProgressValue("SlimesBreed", 10);

            Assert.AreEqual(10, progress.GetProgressValue("SlimesBreed"));
        }

        [Test]
        public void StoryProgress_ReturnsZeroForUnsetProgressValue()
        {
            var progress = new StoryProgress();

            Assert.AreEqual(0, progress.GetProgressValue("NonExistent"));
        }

        [Test]
        public void StoryProgress_CanIncrementProgressValue()
        {
            var progress = new StoryProgress();

            progress.SetProgressValue("SlimesBreed", 5);
            progress.IncrementProgressValue("SlimesBreed", 3);

            Assert.AreEqual(8, progress.GetProgressValue("SlimesBreed"));
        }

        [Test]
        public void StoryEvent_CanBeCreated()
        {
            var storyEvent = new StoryEvent("SE001", "First Discovery", "You've bred your first slime!");

            Assert.IsNotNull(storyEvent);
            Assert.AreEqual("SE001", storyEvent.ID);
            Assert.AreEqual("First Discovery", storyEvent.Title);
            Assert.AreEqual("You've bred your first slime!", storyEvent.Description);
        }

        [Test]
        public void StoryEvent_CanRequireMilestone()
        {
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");
            storyEvent.AddMilestoneRequirement("FirstSlimeBred");

            Assert.AreEqual(1, storyEvent.MilestoneRequirements.Count);
            Assert.Contains("FirstSlimeBred", storyEvent.MilestoneRequirements);
        }

        [Test]
        public void StoryEvent_CanRequireProgressValue()
        {
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");
            storyEvent.AddProgressRequirement("SlimesBreed", 10);

            var requirements = storyEvent.ProgressRequirements;
            Assert.AreEqual(1, requirements.Count);
            Assert.AreEqual(10, requirements["SlimesBreed"]);
        }

        [Test]
        public void StoryEvent_IsAvailableWhenMilestoneRequirementsMet()
        {
            var progress = new StoryProgress();
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");

            storyEvent.AddMilestoneRequirement("FirstSlimeBred");

            // Not available initially
            Assert.IsFalse(storyEvent.IsAvailable(progress));

            // Available after milestone set
            progress.SetMilestone("FirstSlimeBred");
            Assert.IsTrue(storyEvent.IsAvailable(progress));
        }

        [Test]
        public void StoryEvent_IsAvailableWhenProgressRequirementsMet()
        {
            var progress = new StoryProgress();
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");

            storyEvent.AddProgressRequirement("SlimesBreed", 10);

            // Not available initially
            Assert.IsFalse(storyEvent.IsAvailable(progress));

            // Not available when progress is less
            progress.SetProgressValue("SlimesBreed", 5);
            Assert.IsFalse(storyEvent.IsAvailable(progress));

            // Available when progress meets requirement
            progress.SetProgressValue("SlimesBreed", 10);
            Assert.IsTrue(storyEvent.IsAvailable(progress));

            // Available when progress exceeds requirement
            progress.SetProgressValue("SlimesBreed", 15);
            Assert.IsTrue(storyEvent.IsAvailable(progress));
        }

        [Test]
        public void StoryEvent_RequiresAllConditionsToBeAvailable()
        {
            var progress = new StoryProgress();
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");

            storyEvent.AddMilestoneRequirement("FirstSlimeBred");
            storyEvent.AddProgressRequirement("SlimesBreed", 10);

            // Not available with neither condition
            Assert.IsFalse(storyEvent.IsAvailable(progress));

            // Not available with only milestone
            progress.SetMilestone("FirstSlimeBred");
            Assert.IsFalse(storyEvent.IsAvailable(progress));

            // Available with both conditions
            progress.SetProgressValue("SlimesBreed", 10);
            Assert.IsTrue(storyEvent.IsAvailable(progress));
        }

        [Test]
        public void StoryEvent_CanBeTriggered()
        {
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");

            Assert.IsFalse(storyEvent.IsTriggered);

            storyEvent.Trigger();

            Assert.IsTrue(storyEvent.IsTriggered);
        }

        [Test]
        public void StoryEvent_CannotTriggerWhenAlreadyTriggered()
        {
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");

            storyEvent.Trigger();

            Assert.Throws<System.InvalidOperationException>(() => storyEvent.Trigger());
        }

        [Test]
        public void StoryEvent_CanUnlockContent()
        {
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");
            storyEvent.AddContentUnlock("AdvancedBreeding");

            Assert.AreEqual(1, storyEvent.ContentUnlocks.Count);
            Assert.Contains("AdvancedBreeding", storyEvent.ContentUnlocks);
        }

        [Test]
        public void ContentUnlockManager_CanBeCreated()
        {
            var manager = new ContentUnlockManager();

            Assert.IsNotNull(manager);
        }

        [Test]
        public void ContentUnlockManager_CanUnlockContent()
        {
            var manager = new ContentUnlockManager();

            manager.UnlockContent("AdvancedBreeding");

            Assert.IsTrue(manager.IsContentUnlocked("AdvancedBreeding"));
        }

        [Test]
        public void ContentUnlockManager_ReturnsFalseForLockedContent()
        {
            var manager = new ContentUnlockManager();

            Assert.IsFalse(manager.IsContentUnlocked("LockedFeature"));
        }

        [Test]
        public void ContentUnlockManager_CanGetAllUnlockedContent()
        {
            var manager = new ContentUnlockManager();

            manager.UnlockContent("Feature1");
            manager.UnlockContent("Feature2");

            var unlocked = manager.GetUnlockedContent();

            Assert.AreEqual(2, unlocked.Count);
            Assert.Contains("Feature1", unlocked);
            Assert.Contains("Feature2", unlocked);
        }

        [Test]
        public void StoryEvent_AppliesContentUnlocksWhenTriggered()
        {
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");
            var manager = new ContentUnlockManager();

            storyEvent.AddContentUnlock("AdvancedBreeding");
            storyEvent.AddContentUnlock("NewZone");

            storyEvent.Trigger();
            storyEvent.ApplyUnlocks(manager);

            Assert.IsTrue(manager.IsContentUnlocked("AdvancedBreeding"));
            Assert.IsTrue(manager.IsContentUnlocked("NewZone"));
        }

        [Test]
        public void StoryEvent_CannotApplyUnlocksWithoutTriggering()
        {
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");
            var manager = new ContentUnlockManager();

            storyEvent.AddContentUnlock("AdvancedBreeding");

            Assert.Throws<System.InvalidOperationException>(() => storyEvent.ApplyUnlocks(manager));
        }

        [Test]
        public void StoryEventManager_CanBeCreated()
        {
            var manager = new StoryEventManager();

            Assert.IsNotNull(manager);
        }

        [Test]
        public void StoryEventManager_CanAddStoryEvent()
        {
            var manager = new StoryEventManager();
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");

            manager.AddEvent(storyEvent);

            Assert.AreEqual(1, manager.EventCount);
        }

        [Test]
        public void StoryEventManager_CanGetAvailableEvents()
        {
            var progress = new StoryProgress();
            var manager = new StoryEventManager();

            var available = new StoryEvent("SE001", "Available", "Description");
            var locked = new StoryEvent("SE002", "Locked", "Description");

            locked.AddMilestoneRequirement("SomeMilestone");

            manager.AddEvent(available);
            manager.AddEvent(locked);

            var availableEvents = manager.GetAvailableEvents(progress);

            Assert.AreEqual(1, availableEvents.Count);
            Assert.Contains(available, availableEvents);
            Assert.IsFalse(availableEvents.Contains(locked));
        }

        [Test]
        public void StoryEventManager_CanCheckAndTriggerEvents()
        {
            var progress = new StoryProgress();
            var manager = new StoryEventManager();
            var contentManager = new ContentUnlockManager();

            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");
            storyEvent.AddMilestoneRequirement("FirstSlimeBred");
            storyEvent.AddContentUnlock("AdvancedBreeding");

            manager.AddEvent(storyEvent);

            // No events triggered initially
            var triggered = manager.CheckAndTriggerEvents(progress, contentManager);
            Assert.AreEqual(0, triggered.Count);

            // Event triggers when available
            progress.SetMilestone("FirstSlimeBred");
            triggered = manager.CheckAndTriggerEvents(progress, contentManager);

            Assert.AreEqual(1, triggered.Count);
            Assert.Contains(storyEvent, triggered);
            Assert.IsTrue(storyEvent.IsTriggered);
            Assert.IsTrue(contentManager.IsContentUnlocked("AdvancedBreeding"));
        }

        [Test]
        public void StoryEventManager_DoesNotTriggerSameEventTwice()
        {
            var progress = new StoryProgress();
            var manager = new StoryEventManager();
            var contentManager = new ContentUnlockManager();

            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");
            manager.AddEvent(storyEvent);

            // First trigger
            var triggered = manager.CheckAndTriggerEvents(progress, contentManager);
            Assert.AreEqual(1, triggered.Count);

            // Second check - should not trigger again
            triggered = manager.CheckAndTriggerEvents(progress, contentManager);
            Assert.AreEqual(0, triggered.Count);
        }

        [Test]
        public void StoryEventManager_CanGetTriggeredEvents()
        {
            var manager = new StoryEventManager();
            var event1 = new StoryEvent("SE001", "Event 1", "Description 1");
            var event2 = new StoryEvent("SE002", "Event 2", "Description 2");

            manager.AddEvent(event1);
            manager.AddEvent(event2);

            event1.Trigger();

            var triggeredEvents = manager.GetTriggeredEvents();

            Assert.AreEqual(1, triggeredEvents.Count);
            Assert.Contains(event1, triggeredEvents);
            Assert.IsFalse(triggeredEvents.Contains(event2));
        }

        [Test]
        public void StoryProgress_CanGetAllMilestones()
        {
            var progress = new StoryProgress();

            progress.SetMilestone("Milestone1");
            progress.SetMilestone("Milestone2");

            var milestones = progress.GetAllMilestones();

            Assert.AreEqual(2, milestones.Count);
            Assert.Contains("Milestone1", milestones);
            Assert.Contains("Milestone2", milestones);
        }

        [Test]
        public void StoryEvent_IsAvailableWithNoRequirements()
        {
            var progress = new StoryProgress();
            var storyEvent = new StoryEvent("SE001", "Test Event", "Description");

            // Event with no requirements should be immediately available
            Assert.IsTrue(storyEvent.IsAvailable(progress));
        }

        [Test]
        public void ContentUnlockManager_CanLockContent()
        {
            var manager = new ContentUnlockManager();

            manager.UnlockContent("Feature1");
            Assert.IsTrue(manager.IsContentUnlocked("Feature1"));

            manager.LockContent("Feature1");
            Assert.IsFalse(manager.IsContentUnlocked("Feature1"));
        }

        [Test]
        public void StoryEventManager_CanGetAllEvents()
        {
            var manager = new StoryEventManager();
            var event1 = new StoryEvent("SE001", "Event 1", "Description 1");
            var event2 = new StoryEvent("SE002", "Event 2", "Description 2");

            manager.AddEvent(event1);
            manager.AddEvent(event2);

            var allEvents = manager.GetAllEvents();

            Assert.AreEqual(2, allEvents.Count);
            Assert.Contains(event1, allEvents);
            Assert.Contains(event2, allEvents);
        }
    }
}
