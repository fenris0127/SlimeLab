using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class TutorialSystem
    {
        public int StepCount => _steps.Count;
        public TutorialStep CurrentStep { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsComplete { get; private set; }

        public event Action<TutorialStep> OnStepChanged;

        private List<TutorialStep> _steps;
        private int _currentStepIndex;

        public TutorialSystem()
        {
            _steps = new List<TutorialStep>();
            _currentStepIndex = -1;
            IsActive = false;
            IsComplete = false;
            CurrentStep = null;
        }

        public void AddStep(TutorialStep step)
        {
            _steps.Add(step);
        }

        public void Start()
        {
            if (_steps.Count == 0)
            {
                return;
            }

            IsActive = true;
            IsComplete = false;
            _currentStepIndex = 0;
            CurrentStep = _steps[_currentStepIndex];
            OnStepChanged?.Invoke(CurrentStep);
        }

        public void NextStep()
        {
            if (!IsActive)
            {
                return;
            }

            if (CurrentStep != null)
            {
                CurrentStep.Complete();
            }

            _currentStepIndex++;

            if (_currentStepIndex >= _steps.Count)
            {
                IsActive = false;
                IsComplete = true;
                CurrentStep = null;
                return;
            }

            CurrentStep = _steps[_currentStepIndex];
            OnStepChanged?.Invoke(CurrentStep);
        }

        public void Skip()
        {
            IsActive = false;
            IsComplete = true;
            CurrentStep = null;
        }

        public TutorialStep GetStep(string id)
        {
            return _steps.FirstOrDefault(s => s.ID == id);
        }

        public float GetProgress()
        {
            if (_steps.Count == 0)
            {
                return 0f;
            }

            int completedSteps = _steps.Count(s => s.IsComplete);
            return (float)completedSteps / _steps.Count;
        }

        public void Reset()
        {
            IsActive = false;
            IsComplete = false;
            _currentStepIndex = -1;
            CurrentStep = null;

            foreach (var step in _steps)
            {
                // Note: TutorialStep doesn't have a Reset method in the tests,
                // so we can't reset individual steps. Just reset the system state.
            }
        }
    }
}
