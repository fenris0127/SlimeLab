using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class BonusManager
    {
        private List<PermanentBonus> _activeBonuses;

        public BonusManager()
        {
            _activeBonuses = new List<PermanentBonus>();
        }

        public void ApplyBonus(PermanentBonus bonus)
        {
            _activeBonuses.Add(bonus);
        }

        public void RemoveBonus(PermanentBonus bonus)
        {
            _activeBonuses.Remove(bonus);
        }

        public float GetTotalBonus(BonusType bonusType)
        {
            return _activeBonuses
                .Where(b => b.Type == bonusType && b.IsPercentage)
                .Sum(b => b.Value);
        }

        public float GetFlatBonus(BonusType bonusType)
        {
            return _activeBonuses
                .Where(b => b.Type == bonusType && !b.IsPercentage)
                .Sum(b => b.Value);
        }

        public float GetMultiplier(BonusType bonusType)
        {
            // Multiplier = 1.0 + total percentage bonuses
            return 1.0f + GetTotalBonus(bonusType);
        }

        public List<PermanentBonus> GetActiveBonuses()
        {
            return new List<PermanentBonus>(_activeBonuses);
        }

        public List<PermanentBonus> GetBonusesByType(BonusType bonusType)
        {
            return _activeBonuses
                .Where(b => b.Type == bonusType)
                .ToList();
        }

        public void ClearAll()
        {
            _activeBonuses.Clear();
        }
    }
}
