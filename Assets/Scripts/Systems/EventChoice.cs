namespace SlimeLab.Systems
{
    public class EventChoice
    {
        public string ID { get; private set; }
        public string Description { get; private set; }
        public EventOutcome Outcome { get; private set; }

        public EventChoice(string id, string description)
        {
            ID = id;
            Description = description;
        }

        public void SetOutcome(EventOutcome outcome)
        {
            Outcome = outcome;
        }
    }
}
