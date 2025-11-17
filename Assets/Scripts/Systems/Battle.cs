using System;
using System.Collections.Generic;
using System.Linq;
using SlimeLab.Core;

namespace SlimeLab.Systems
{
    public class Battle
    {
        public int CurrentTurn { get; private set; }

        private List<Slime> _playerTeam;
        private List<Slime> _enemyTeam;
        private List<Slime> _turnOrder;

        public Battle(List<Slime> playerTeam, List<Slime> enemyTeam)
        {
            _playerTeam = new List<Slime>(playerTeam);
            _enemyTeam = new List<Slime>(enemyTeam);
            CurrentTurn = 0;

            CalculateTurnOrder();
        }

        private void CalculateTurnOrder()
        {
            // Combine all combatants and sort by speed (descending)
            _turnOrder = new List<Slime>();
            _turnOrder.AddRange(_playerTeam);
            _turnOrder.AddRange(_enemyTeam);

            // Sort by speed, highest first
            _turnOrder = _turnOrder
                .Where(s => s.Stats.IsAlive())
                .OrderByDescending(s => s.Stats.Speed)
                .ToList();
        }

        public void NextTurn()
        {
            CurrentTurn++;

            // Recalculate turn order to remove dead slimes
            CalculateTurnOrder();
        }

        public List<Slime> GetTurnOrder()
        {
            return new List<Slime>(_turnOrder);
        }

        public Slime GetCurrentActor()
        {
            if (_turnOrder.Count == 0)
            {
                return null;
            }

            // Get current actor based on turn number modulo turn order size
            int actorIndex = CurrentTurn % _turnOrder.Count;
            return _turnOrder[actorIndex];
        }

        public void ExecuteAttack(Slime attacker, Slime defender)
        {
            // Calculate base damage
            int baseDamage = attacker.Stats.Attack - defender.Stats.Defense;

            // Minimum 1 damage
            if (baseDamage < 1)
            {
                baseDamage = 1;
            }

            // Apply elemental advantage multiplier
            float multiplier = ElementalAdvantage.GetMultiplier(attacker.Element, defender.Element);
            int finalDamage = (int)(baseDamage * multiplier);

            // Ensure at least 1 damage
            if (finalDamage < 1)
            {
                finalDamage = 1;
            }

            // Apply damage to defender
            defender.Stats.TakeDamage(finalDamage);

            // Update turn order if defender died
            if (!defender.Stats.IsAlive())
            {
                CalculateTurnOrder();
            }
        }

        public bool IsOver()
        {
            // Battle is over when one team has no alive members
            bool playerTeamAlive = _playerTeam.Any(s => s.Stats.IsAlive());
            bool enemyTeamAlive = _enemyTeam.Any(s => s.Stats.IsAlive());

            return !playerTeamAlive || !enemyTeamAlive;
        }

        public BattleTeam GetWinner()
        {
            if (!IsOver())
            {
                return BattleTeam.None;
            }

            bool playerTeamAlive = _playerTeam.Any(s => s.Stats.IsAlive());

            if (playerTeamAlive)
            {
                return BattleTeam.Player;
            }
            else
            {
                return BattleTeam.Enemy;
            }
        }
    }
}
