namespace SlimeLab.Systems
{
    public enum ResearchState
    {
        Locked,      // Not available for research (prerequisites not met)
        Available,   // Available for research but not started
        InProgress,  // Currently being researched
        Completed    // Research finished
    }
}
