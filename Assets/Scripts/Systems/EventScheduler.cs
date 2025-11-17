using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class EventScheduler
    {
        public int EventCount => _events.Count;

        private List<RandomEvent> _events;
        private Random _random;

        public EventScheduler(int? seed = null)
        {
            _events = new List<RandomEvent>();
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public void AddEvent(RandomEvent randomEvent)
        {
            _events.Add(randomEvent);
        }

        public RandomEvent CheckForEvent()
        {
            foreach (var randomEvent in _events)
            {
                double roll = _random.NextDouble();
                if (roll < randomEvent.Probability)
                {
                    return randomEvent;
                }
            }

            return null;
        }

        public void RemoveTriggeredEvents()
        {
            _events.RemoveAll(e => e.IsTriggered);
        }

        public List<RandomEvent> GetAllEvents()
        {
            return new List<RandomEvent>(_events);
        }

        public void ClearAllEvents()
        {
            _events.Clear();
        }
    }
}
