using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class StoryEventManager
    {
        public int EventCount => _events.Count;

        private List<StoryEvent> _events;

        public StoryEventManager()
        {
            _events = new List<StoryEvent>();
        }

        public void AddEvent(StoryEvent storyEvent)
        {
            _events.Add(storyEvent);
        }

        public List<StoryEvent> GetAvailableEvents(StoryProgress progress)
        {
            return _events
                .Where(e => !e.IsTriggered && e.IsAvailable(progress))
                .ToList();
        }

        public List<StoryEvent> CheckAndTriggerEvents(StoryProgress progress, ContentUnlockManager contentManager)
        {
            var triggeredEvents = new List<StoryEvent>();
            var availableEvents = GetAvailableEvents(progress);

            foreach (var storyEvent in availableEvents)
            {
                storyEvent.Trigger();
                storyEvent.ApplyUnlocks(contentManager);
                triggeredEvents.Add(storyEvent);
            }

            return triggeredEvents;
        }

        public List<StoryEvent> GetTriggeredEvents()
        {
            return _events
                .Where(e => e.IsTriggered)
                .ToList();
        }

        public List<StoryEvent> GetAllEvents()
        {
            return new List<StoryEvent>(_events);
        }
    }
}
