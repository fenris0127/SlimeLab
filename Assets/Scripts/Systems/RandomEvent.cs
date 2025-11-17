using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class RandomEvent
    {
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public float Probability { get; private set; }
        public bool IsTriggered { get; private set; }
        public bool IsResolved { get; private set; }
        public List<EventChoice> Choices { get; private set; }

        public RandomEvent(string id, string title, string description, float probability = 0.1f)
        {
            ID = id;
            Title = title;
            Description = description;
            Probability = probability;
            IsTriggered = false;
            IsResolved = false;
            Choices = new List<EventChoice>();
        }

        public void AddChoice(EventChoice choice)
        {
            Choices.Add(choice);
        }

        public void Trigger()
        {
            IsTriggered = true;
        }

        public void Resolve(EventChoice choice)
        {
            if (!IsTriggered)
            {
                throw new InvalidOperationException("Event must be triggered before it can be resolved");
            }

            if (!Choices.Contains(choice))
            {
                throw new InvalidOperationException("Choice is not valid for this event");
            }

            IsResolved = true;
        }

        public bool HasChoice(EventChoice choice)
        {
            return Choices.Contains(choice);
        }
    }
}
