using System;

namespace SlimeLab.Systems
{
    public class MusicTransition
    {
        public string FromMusicID { get; private set; }
        public string ToMusicID { get; private set; }
        public float Duration { get; private set; }
        public float Progress { get; private set; }
        public bool IsComplete { get; private set; }
        public TransitionCurve Curve { get; private set; }

        private float _elapsedTime;

        public MusicTransition(string fromMusicID, string toMusicID, float duration, TransitionCurve curve = TransitionCurve.Linear)
        {
            FromMusicID = fromMusicID;
            ToMusicID = toMusicID;
            Duration = duration;
            Curve = curve;
            Progress = 0f;
            IsComplete = false;
            _elapsedTime = 0f;
        }

        public void Update(float deltaTime)
        {
            if (IsComplete)
            {
                return;
            }

            _elapsedTime += deltaTime;

            if (_elapsedTime >= Duration)
            {
                Progress = 1.0f;
                IsComplete = true;
            }
            else
            {
                Progress = Math.Min(1.0f, _elapsedTime / Duration);
            }
        }
    }
}
