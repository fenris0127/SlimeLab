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

        private Dictionary<string, Slime> _team;

        public Expedition(Zone targetZone, int maxTeamSize = 4)
        {
            TargetZone = targetZone;
            MaxTeamSize = maxTeamSize;
            _team = new Dictionary<string, Slime>();
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
    }
}
