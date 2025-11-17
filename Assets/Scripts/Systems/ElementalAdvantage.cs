using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public static class ElementalAdvantage
    {
        public static float GetMultiplier(ElementType attackerElement, ElementType defenderElement)
        {
            // Neutral has no advantages or disadvantages
            if (attackerElement == ElementType.Neutral || defenderElement == ElementType.Neutral)
            {
                return 1.0f;
            }

            // Check for elemental advantage
            if (HasAdvantage(attackerElement, defenderElement))
            {
                return 1.5f; // 50% bonus damage
            }

            // Check for elemental disadvantage
            if (HasAdvantage(defenderElement, attackerElement))
            {
                return 0.75f; // 25% reduced damage
            }

            // Same element or no advantage
            return 1.0f;
        }

        private static bool HasAdvantage(ElementType attacker, ElementType defender)
        {
            // Water > Fire
            if (attacker == ElementType.Water && defender == ElementType.Fire)
            {
                return true;
            }

            // Fire > Electric
            if (attacker == ElementType.Fire && defender == ElementType.Electric)
            {
                return true;
            }

            // Electric > Water
            if (attacker == ElementType.Electric && defender == ElementType.Water)
            {
                return true;
            }

            return false;
        }
    }
}
