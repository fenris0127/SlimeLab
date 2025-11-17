using System.Collections.Generic;

namespace SlimeLab.Systems
{
    public class MemoryManager
    {
        private Dictionary<string, long> _allocations;

        public MemoryManager()
        {
            _allocations = new Dictionary<string, long>();
        }

        public void RecordAllocation(string type, long bytes)
        {
            if (!_allocations.ContainsKey(type))
            {
                _allocations[type] = 0;
            }

            _allocations[type] += bytes;
        }

        public void RecordDeallocation(string type, long bytes)
        {
            if (!_allocations.ContainsKey(type))
            {
                return;
            }

            _allocations[type] -= bytes;

            if (_allocations[type] <= 0)
            {
                _allocations.Remove(type);
            }
        }

        public long GetTotalAllocated(string type)
        {
            return _allocations.ContainsKey(type) ? _allocations[type] : 0;
        }

        public long GetTotalMemoryUsage()
        {
            long total = 0;
            foreach (var allocation in _allocations.Values)
            {
                total += allocation;
            }
            return total;
        }

        public bool HasPotentialLeak(string type, long threshold)
        {
            long allocated = GetTotalAllocated(type);
            return allocated > threshold;
        }

        public Dictionary<string, long> GetAllocationsByType()
        {
            return new Dictionary<string, long>(_allocations);
        }

        public void Clear()
        {
            _allocations.Clear();
        }
    }
}
