using NUnit.Framework;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class PolishTests
    {
        [Test]
        public void FeedbackSystem_CanBeCreated()
        {
            var feedbackSystem = new FeedbackSystem();

            Assert.IsNotNull(feedbackSystem);
        }

        [Test]
        public void FeedbackSystem_CanRegisterFeedback()
        {
            var feedbackSystem = new FeedbackSystem();

            feedbackSystem.RegisterFeedback("ButtonClick", FeedbackType.Audio, "sfx_click");

            Assert.AreEqual(1, feedbackSystem.RegisteredFeedbackCount);
        }

        [Test]
        public void FeedbackSystem_CanTriggerFeedback()
        {
            var feedbackSystem = new FeedbackSystem();

            bool triggered = false;
            feedbackSystem.OnFeedbackTriggered += (action, type, data) => { triggered = true; };

            feedbackSystem.RegisterFeedback("ButtonClick", FeedbackType.Audio, "sfx_click");
            feedbackSystem.TriggerFeedback("ButtonClick");

            Assert.IsTrue(triggered);
        }

        [Test]
        public void FeedbackSystem_CanRegisterMultipleFeedbackForAction()
        {
            var feedbackSystem = new FeedbackSystem();

            feedbackSystem.RegisterFeedback("ButtonClick", FeedbackType.Audio, "sfx_click");
            feedbackSystem.RegisterFeedback("ButtonClick", FeedbackType.Visual, "flash");

            var feedbacks = feedbackSystem.GetFeedbacks("ButtonClick");

            Assert.AreEqual(2, feedbacks.Count);
        }

        [Test]
        public void FeedbackType_HasExpectedValues()
        {
            var audio = FeedbackType.Audio;
            var visual = FeedbackType.Visual;
            var haptic = FeedbackType.Haptic;

            Assert.IsNotNull(audio);
            Assert.IsNotNull(visual);
            Assert.IsNotNull(haptic);
        }

        [Test]
        public void TutorialSystem_CanBeCreated()
        {
            var tutorialSystem = new TutorialSystem();

            Assert.IsNotNull(tutorialSystem);
        }

        [Test]
        public void TutorialStep_CanBeCreated()
        {
            var step = new TutorialStep("Step1", "Welcome to SlimeLab!", "This is your laboratory.");

            Assert.IsNotNull(step);
            Assert.AreEqual("Step1", step.ID);
            Assert.AreEqual("Welcome to SlimeLab!", step.Title);
            Assert.AreEqual("This is your laboratory.", step.Description);
        }

        [Test]
        public void TutorialStep_StartsIncomplete()
        {
            var step = new TutorialStep("Step1", "Title", "Description");

            Assert.IsFalse(step.IsComplete);
        }

        [Test]
        public void TutorialStep_CanBeCompleted()
        {
            var step = new TutorialStep("Step1", "Title", "Description");

            step.Complete();

            Assert.IsTrue(step.IsComplete);
        }

        [Test]
        public void TutorialSystem_CanAddStep()
        {
            var tutorialSystem = new TutorialSystem();
            var step = new TutorialStep("Step1", "Title", "Description");

            tutorialSystem.AddStep(step);

            Assert.AreEqual(1, tutorialSystem.StepCount);
        }

        [Test]
        public void TutorialSystem_CanGetCurrentStep()
        {
            var tutorialSystem = new TutorialSystem();
            var step1 = new TutorialStep("Step1", "First", "First step");
            var step2 = new TutorialStep("Step2", "Second", "Second step");

            tutorialSystem.AddStep(step1);
            tutorialSystem.AddStep(step2);

            tutorialSystem.Start();

            Assert.AreEqual(step1, tutorialSystem.CurrentStep);
        }

        [Test]
        public void TutorialSystem_CanAdvanceToNextStep()
        {
            var tutorialSystem = new TutorialSystem();
            var step1 = new TutorialStep("Step1", "First", "First step");
            var step2 = new TutorialStep("Step2", "Second", "Second step");

            tutorialSystem.AddStep(step1);
            tutorialSystem.AddStep(step2);

            tutorialSystem.Start();
            tutorialSystem.NextStep();

            Assert.AreEqual(step2, tutorialSystem.CurrentStep);
        }

        [Test]
        public void TutorialSystem_CompletesWhenAllStepsDone()
        {
            var tutorialSystem = new TutorialSystem();
            var step1 = new TutorialStep("Step1", "First", "First step");
            var step2 = new TutorialStep("Step2", "Second", "Second step");

            tutorialSystem.AddStep(step1);
            tutorialSystem.AddStep(step2);

            tutorialSystem.Start();
            tutorialSystem.NextStep();
            tutorialSystem.NextStep();

            Assert.IsTrue(tutorialSystem.IsComplete);
        }

        [Test]
        public void TutorialSystem_CanCheckIfActive()
        {
            var tutorialSystem = new TutorialSystem();
            var step = new TutorialStep("Step1", "Title", "Description");

            tutorialSystem.AddStep(step);

            Assert.IsFalse(tutorialSystem.IsActive);

            tutorialSystem.Start();

            Assert.IsTrue(tutorialSystem.IsActive);
        }

        [Test]
        public void TutorialSystem_CanSkip()
        {
            var tutorialSystem = new TutorialSystem();
            var step1 = new TutorialStep("Step1", "First", "First step");
            var step2 = new TutorialStep("Step2", "Second", "Second step");

            tutorialSystem.AddStep(step1);
            tutorialSystem.AddStep(step2);

            tutorialSystem.Start();
            tutorialSystem.Skip();

            Assert.IsTrue(tutorialSystem.IsComplete);
            Assert.IsFalse(tutorialSystem.IsActive);
        }

        [Test]
        public void TutorialStep_CanHaveAction()
        {
            var step = new TutorialStep("Step1", "Title", "Description");

            bool actionTriggered = false;
            step.SetAction(() => { actionTriggered = true; });

            step.ExecuteAction();

            Assert.IsTrue(actionTriggered);
        }

        [Test]
        public void FeedbackSystem_CanClearFeedback()
        {
            var feedbackSystem = new FeedbackSystem();

            feedbackSystem.RegisterFeedback("Action1", FeedbackType.Audio, "sfx");
            Assert.AreEqual(1, feedbackSystem.RegisteredFeedbackCount);

            feedbackSystem.ClearFeedback("Action1");

            Assert.AreEqual(0, feedbackSystem.RegisteredFeedbackCount);
        }

        [Test]
        public void FeedbackSystem_DoesNotTriggerUnregisteredAction()
        {
            var feedbackSystem = new FeedbackSystem();

            bool triggered = false;
            feedbackSystem.OnFeedbackTriggered += (action, type, data) => { triggered = true; };

            feedbackSystem.TriggerFeedback("NonExistent");

            Assert.IsFalse(triggered);
        }

        [Test]
        public void TutorialSystem_CanGetStepByID()
        {
            var tutorialSystem = new TutorialSystem();
            var step = new TutorialStep("Step1", "Title", "Description");

            tutorialSystem.AddStep(step);

            var retrieved = tutorialSystem.GetStep("Step1");

            Assert.AreEqual(step, retrieved);
        }

        [Test]
        public void TutorialSystem_CanGetProgress()
        {
            var tutorialSystem = new TutorialSystem();
            var step1 = new TutorialStep("Step1", "First", "First step");
            var step2 = new TutorialStep("Step2", "Second", "Second step");
            var step3 = new TutorialStep("Step3", "Third", "Third step");

            tutorialSystem.AddStep(step1);
            tutorialSystem.AddStep(step2);
            tutorialSystem.AddStep(step3);

            tutorialSystem.Start();
            tutorialSystem.NextStep(); // On step 2

            float progress = tutorialSystem.GetProgress();

            // 1 step completed out of 3 = 33.3%
            Assert.Greater(progress, 0.3f);
            Assert.Less(progress, 0.4f);
        }

        [Test]
        public void FeedbackSystem_CanGetAllActions()
        {
            var feedbackSystem = new FeedbackSystem();

            feedbackSystem.RegisterFeedback("Action1", FeedbackType.Audio, "sfx1");
            feedbackSystem.RegisterFeedback("Action2", FeedbackType.Visual, "flash");

            var actions = feedbackSystem.GetAllActions();

            Assert.AreEqual(2, actions.Count);
            Assert.Contains("Action1", actions);
            Assert.Contains("Action2", actions);
        }

        [Test]
        public void TutorialStep_CanHaveHighlightTarget()
        {
            var step = new TutorialStep("Step1", "Title", "Description");
            step.SetHighlightTarget("Button_Feed");

            Assert.AreEqual("Button_Feed", step.HighlightTarget);
        }

        [Test]
        public void TutorialSystem_NotifiesOnStepChange()
        {
            var tutorialSystem = new TutorialSystem();
            var step1 = new TutorialStep("Step1", "First", "First step");
            var step2 = new TutorialStep("Step2", "Second", "Second step");

            tutorialSystem.AddStep(step1);
            tutorialSystem.AddStep(step2);

            bool notified = false;
            tutorialSystem.OnStepChanged += (step) => { notified = true; };

            tutorialSystem.Start();
            tutorialSystem.NextStep();

            Assert.IsTrue(notified);
        }

        [Test]
        public void FeedbackSystem_CanEnableDisable()
        {
            var feedbackSystem = new FeedbackSystem();

            Assert.IsTrue(feedbackSystem.IsEnabled);

            feedbackSystem.SetEnabled(false);

            Assert.IsFalse(feedbackSystem.IsEnabled);
        }

        [Test]
        public void FeedbackSystem_DoesNotTriggerWhenDisabled()
        {
            var feedbackSystem = new FeedbackSystem();

            bool triggered = false;
            feedbackSystem.OnFeedbackTriggered += (action, type, data) => { triggered = true; };

            feedbackSystem.RegisterFeedback("Action1", FeedbackType.Audio, "sfx");
            feedbackSystem.SetEnabled(false);
            feedbackSystem.TriggerFeedback("Action1");

            Assert.IsFalse(triggered);
        }

        [Test]
        public void TutorialSystem_CanReset()
        {
            var tutorialSystem = new TutorialSystem();
            var step = new TutorialStep("Step1", "Title", "Description");

            tutorialSystem.AddStep(step);
            tutorialSystem.Start();
            tutorialSystem.NextStep();

            tutorialSystem.Reset();

            Assert.IsFalse(tutorialSystem.IsActive);
            Assert.IsFalse(tutorialSystem.IsComplete);
        }

        [Test]
        public void TutorialStep_CanBeSkipped()
        {
            var step = new TutorialStep("Step1", "Title", "Description");
            step.SetSkippable(true);

            Assert.IsTrue(step.IsSkippable);
        }

        [Test]
        public void TutorialStep_DefaultNotSkippable()
        {
            var step = new TutorialStep("Step1", "Title", "Description");

            Assert.IsFalse(step.IsSkippable);
        }
    }
}
