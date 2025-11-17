using System;
using System.Collections.Generic;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class Laboratory
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public int MaxCapacity { get; private set; }
        public int SlimeCount => _slimeStorage.Count;
        public int ContainmentUnitCount => _containmentUnits.Count;

        private Dictionary<string, Slime> _slimeStorage;
        private List<ContainmentUnit> _containmentUnits;

        public Laboratory(string name, int maxCapacity = 10)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            MaxCapacity = maxCapacity;
            _slimeStorage = new Dictionary<string, Slime>();
            _containmentUnits = new List<ContainmentUnit>();
        }

        public void AddSlime(Slime slime)
        {
            if (_slimeStorage.Count >= MaxCapacity)
            {
                throw new CapacityExceededException(MaxCapacity, _slimeStorage.Count);
            }

            _slimeStorage[slime.ID] = slime;
        }

        public void RemoveSlime(string slimeID)
        {
            if (_slimeStorage.ContainsKey(slimeID))
            {
                _slimeStorage.Remove(slimeID);
            }
        }

        public bool ContainsSlime(string slimeID)
        {
            return _slimeStorage.ContainsKey(slimeID);
        }

        public Slime GetSlime(string slimeID)
        {
            return _slimeStorage.ContainsKey(slimeID) ? _slimeStorage[slimeID] : null;
        }

        public void AddContainmentUnit(ContainmentUnit unit)
        {
            _containmentUnits.Add(unit);
        }

        public void RemoveContainmentUnit(int index)
        {
            if (index >= 0 && index < _containmentUnits.Count)
            {
                _containmentUnits.RemoveAt(index);
            }
        }

        public ContainmentUnit GetContainmentUnit(int index)
        {
            if (index >= 0 && index < _containmentUnits.Count)
            {
                return _containmentUnits[index];
            }
            return null;
        }

        public List<ContainmentUnit> GetAllContainmentUnits()
        {
            return new List<ContainmentUnit>(_containmentUnits);
        }

        public List<Slime> GetAllSlimes()
        {
            return new List<Slime>(_slimeStorage.Values);
        }
    }
}
