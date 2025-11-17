using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.UI
{
    public class LabViewData
    {
        public int Capacity { get; private set; }
        public int CurrentSlimeCount { get; private set; }
        public int AvailableSpace { get; private set; }
        public float OccupancyPercentage { get; private set; }
        public bool IsFull { get; private set; }

        private Laboratory _laboratory;

        public LabViewData(Laboratory laboratory)
        {
            _laboratory = laboratory;
            Refresh(laboratory);
        }

        public void Refresh(Laboratory laboratory)
        {
            _laboratory = laboratory;
            Capacity = laboratory.Capacity;
            CurrentSlimeCount = laboratory.SlimeCount;
            AvailableSpace = laboratory.AvailableSpace;

            if (Capacity > 0)
            {
                OccupancyPercentage = (float)CurrentSlimeCount / Capacity;
            }
            else
            {
                OccupancyPercentage = 0f;
            }

            IsFull = laboratory.IsFull();
        }

        public List<Slime> GetSlimes()
        {
            return _laboratory.GetAllSlimes();
        }

        public List<ContainmentUnit> GetContainmentUnits()
        {
            return _laboratory.GetAllContainmentUnits();
        }
    }
}
