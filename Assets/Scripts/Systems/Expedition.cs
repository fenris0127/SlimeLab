using System;
using System.Collections.Generic;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class Expedition
    {
        public Zone TargetZone { get; private set; }
        public int MaxTeamSize { get; private set; }
        public int TeamSize => _team.Count;
        public ExpeditionStatus Status { get; private set; }
        public List<Resource> CollectedResources => new List<Resource>(_collectedResources);

        private Dictionary<string, Slime> _team;
        private List<Resource> _collectedResources;

        public Expedition(Zone targetZone, int maxTeamSize = 4)
        {
            TargetZone = targetZone;
            MaxTeamSize = maxTeamSize;
            _team = new Dictionary<string, Slime>();
            _collectedResources = new List<Resource>();
            Status = ExpeditionStatus.Preparing;
        }

        public void AddSlime(Slime slime)
        {
            // Cannot add slimes after expedition has started
            if (Status != ExpeditionStatus.Preparing)
            {
                throw new InvalidOperationException("Cannot add slimes to an expedition that has already started");
            }

            // Check if slime already in team
            if (_team.ContainsKey(slime.ID))
            {
                throw new InvalidOperationException($"Slime {slime.Name} is already in the expedition team");
            }

            // Check team size limit
            if (_team.Count >= MaxTeamSize)
            {
                throw new InvalidOperationException($"Expedition team is full. Maximum team size: {MaxTeamSize}");
            }

            // Validate slime meets zone requirements
            if (TargetZone.Requirement != null && !TargetZone.Requirement.IsMet(slime))
            {
                throw new InvalidOperationException($"Slime {slime.Name} does not meet the requirements for {TargetZone.Name}");
            }

            _team[slime.ID] = slime;
        }

        public void RemoveSlime(string slimeID)
        {
            if (_team.ContainsKey(slimeID))
            {
                _team.Remove(slimeID);
            }
        }

        public bool HasSlime(string slimeID)
        {
            return _team.ContainsKey(slimeID);
        }

        public List<Slime> GetTeam()
        {
            return new List<Slime>(_team.Values);
        }

        public void Start(Laboratory laboratory)
        {
            // Cannot start with empty team
            if (_team.Count == 0)
            {
                throw new InvalidOperationException("Cannot start expedition with empty team");
            }

            // Remove all team members from laboratory
            foreach (var slime in _team.Values)
            {
                laboratory.RemoveSlime(slime.ID);
            }

            // Set status to active
            Status = ExpeditionStatus.Active;
        }

        public void AddCollectedResource(Resource resource)
        {
            _collectedResources.Add(resource);
        }

        public int GetResourcesByType(ResourceType resourceType)
        {
            int total = 0;
            foreach (var resource in _collectedResources)
            {
                if (resource.Type == resourceType)
                {
                    total += resource.Amount;
                }
            }
            return total;
        }

        public List<Resource> Complete(Laboratory laboratory)
        {
            // Can only complete if expedition is active
            if (Status != ExpeditionStatus.Active)
            {
                throw new InvalidOperationException("Expedition must be active to complete");
            }

            // Return all team members to laboratory
            foreach (var slime in _team.Values)
            {
                laboratory.AddSlime(slime);
            }

            // Set status to completed
            Status = ExpeditionStatus.Completed;

            // Return collected resources
            return new List<Resource>(_collectedResources);
        }
    }
}
