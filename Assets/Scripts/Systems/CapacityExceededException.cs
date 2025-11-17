using System;

namespace SlimeLab.Systems
{
    public class CapacityExceededException : Exception
    {
        public int MaxCapacity { get; private set; }
        public int CurrentCount { get; private set; }

        public CapacityExceededException(int maxCapacity, int currentCount)
            : base($"Laboratory capacity exceeded. Maximum: {maxCapacity}, Current: {currentCount}")
        {
            MaxCapacity = maxCapacity;
            CurrentCount = currentCount;
        }
    }
}
