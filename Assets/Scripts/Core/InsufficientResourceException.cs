using System;

namespace SlimeLab.Core
{
    public class InsufficientResourceException : Exception
    {
        public ResourceType ResourceType { get; private set; }
        public int Required { get; private set; }
        public int Available { get; private set; }

        public InsufficientResourceException(ResourceType resourceType, int required, int available)
            : base($"Insufficient {resourceType}. Required: {required}, Available: {available}")
        {
            ResourceType = resourceType;
            Required = required;
            Available = available;
        }
    }
}
