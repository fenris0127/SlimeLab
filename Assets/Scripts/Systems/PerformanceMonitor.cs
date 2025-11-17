using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class PerformanceMonitor
    {
        public int FrameCount => _frameTimes.Count;

        private List<float> _frameTimes;
        private int _maxSamples;

        public PerformanceMonitor(int maxSamples = 60)
        {
            _maxSamples = maxSamples;
            _frameTimes = new List<float>();
        }

        public void RecordFrame(float deltaTimeMs)
        {
            _frameTimes.Add(deltaTimeMs);

            // Maintain sliding window
            if (_frameTimes.Count > _maxSamples)
            {
                _frameTimes.RemoveAt(0);
            }
        }

        public float GetAverageFPS()
        {
            if (_frameTimes.Count == 0)
            {
                return 0f;
            }

            float avgFrameTime = _frameTimes.Average();
            return avgFrameTime > 0 ? 1000f / avgFrameTime : 0f;
        }

        public float GetMinFPS()
        {
            if (_frameTimes.Count == 0)
            {
                return 0f;
            }

            float maxFrameTime = _frameTimes.Max();
            return maxFrameTime > 0 ? 1000f / maxFrameTime : 0f;
        }

        public float GetMaxFPS()
        {
            if (_frameTimes.Count == 0)
            {
                return 0f;
            }

            float minFrameTime = _frameTimes.Min();
            return minFrameTime > 0 ? 1000f / minFrameTime : float.MaxValue;
        }

        public float GetCurrentFPS()
        {
            if (_frameTimes.Count == 0)
            {
                return 0f;
            }

            float lastFrameTime = _frameTimes[_frameTimes.Count - 1];
            return lastFrameTime > 0 ? 1000f / lastFrameTime : float.MaxValue;
        }

        public void Reset()
        {
            _frameTimes.Clear();
        }
    }
}
