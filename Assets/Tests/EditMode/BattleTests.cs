using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class BattleTests
    {
        [Test]
        public void Battle_CanBeCreated()
        {
            var playerTeam = new List<Slime> { new Slime("Player Slime", ElementType.Fire) };
            var enemyTeam = new List<Slime> { new Slime("Enemy Slime", ElementType.Water) };

            var battle = new Battle(playerTeam, enemyTeam);

            Assert.IsNotNull(battle);
        }

        [Test]
        public void Battle_HasTurnCounter()
        {
            var playerTeam = new List<Slime> { new Slime("Player Slime", ElementType.Fire) };
            var enemyTeam = new List<Slime> { new Slime("Enemy Slime", ElementType.Water) };
            var battle = new Battle(playerTeam, enemyTeam);

            Assert.AreEqual(0, battle.CurrentTurn);
        }

        [Test]
        public void Battle_CanAdvanceTurn()
        {
            var playerTeam = new List<Slime> { new Slime("Player Slime", ElementType.Fire) };
            var enemyTeam = new List<Slime> { new Slime("Enemy Slime", ElementType.Water) };
            var battle = new Battle(playerTeam, enemyTeam);

            battle.NextTurn();

            Assert.AreEqual(1, battle.CurrentTurn);
        }

        [Test]
        public void Battle_DeterminesTurnOrderBySpeed()
        {
            var fastSlime = new Slime("Fast Slime", ElementType.Electric);
            var slowSlime = new Slime("Slow Slime", ElementType.Water);

            var playerTeam = new List<Slime> { slowSlime };
            var enemyTeam = new List<Slime> { fastSlime };

            var battle = new Battle(playerTeam, enemyTeam);
            var turnOrder = battle.GetTurnOrder();

            // Electric slime has higher base speed (13) than Water slime (8)
            Assert.AreEqual(fastSlime, turnOrder[0]);
            Assert.AreEqual(slowSlime, turnOrder[1]);
        }

        [Test]
        public void Battle_TurnOrderIncludesAllCombatants()
        {
            var slime1 = new Slime("Slime 1", ElementType.Fire);
            var slime2 = new Slime("Slime 2", ElementType.Water);
            var slime3 = new Slime("Slime 3", ElementType.Electric);

            var playerTeam = new List<Slime> { slime1, slime2 };
            var enemyTeam = new List<Slime> { slime3 };

            var battle = new Battle(playerTeam, enemyTeam);
            var turnOrder = battle.GetTurnOrder();

            Assert.AreEqual(3, turnOrder.Count);
            Assert.Contains(slime1, turnOrder);
            Assert.Contains(slime2, turnOrder);
            Assert.Contains(slime3, turnOrder);
        }

        [Test]
        public void Battle_GetCurrentActorReturnsSlimeBasedOnTurnOrder()
        {
            var fastSlime = new Slime("Fast Slime", ElementType.Electric);
            var slowSlime = new Slime("Slow Slime", ElementType.Water);

            var playerTeam = new List<Slime> { slowSlime };
            var enemyTeam = new List<Slime> { fastSlime };

            var battle = new Battle(playerTeam, enemyTeam);

            // First turn should be the fast slime
            var currentActor = battle.GetCurrentActor();
            Assert.AreEqual(fastSlime, currentActor);

            battle.NextTurn();

            // Second turn should be the slow slime
            currentActor = battle.GetCurrentActor();
            Assert.AreEqual(slowSlime, currentActor);
        }

        [Test]
        public void Battle_CanExecuteBasicAttack()
        {
            var attacker = new Slime("Attacker", ElementType.Fire);
            var defender = new Slime("Defender", ElementType.Water);

            var playerTeam = new List<Slime> { attacker };
            var enemyTeam = new List<Slime> { defender };

            var battle = new Battle(playerTeam, enemyTeam);

            int initialHP = defender.Stats.HP;

            battle.ExecuteAttack(attacker, defender);

            // Defender should have taken damage
            Assert.Less(defender.Stats.HP, initialHP);
        }

        [Test]
        public void Battle_AttackDamageBasedOnAttackerAttackAndDefenderDefense()
        {
            var strongAttacker = new Slime("Strong", ElementType.Fire);
            var weakDefender = new Slime("Weak", ElementType.Neutral);

            var playerTeam = new List<Slime> { strongAttacker };
            var enemyTeam = new List<Slime> { weakDefender };

            var battle = new Battle(playerTeam, enemyTeam);

            int initialHP = weakDefender.Stats.HP;
            int attackPower = strongAttacker.Stats.Attack;
            int defense = weakDefender.Stats.Defense;

            battle.ExecuteAttack(strongAttacker, weakDefender);

            int expectedDamage = attackPower - defense;
            if (expectedDamage < 1) expectedDamage = 1; // Minimum damage

            Assert.AreEqual(initialHP - expectedDamage, weakDefender.Stats.HP);
        }

        [Test]
        public void Battle_AttackAlwaysDealsMinimumOneDamage()
        {
            var weakAttacker = new Slime("Weak", ElementType.Neutral);
            var strongDefender = new Slime("Strong", ElementType.Water);

            var playerTeam = new List<Slime> { weakAttacker };
            var enemyTeam = new List<Slime> { strongDefender };

            var battle = new Battle(playerTeam, enemyTeam);

            int initialHP = strongDefender.Stats.HP;

            battle.ExecuteAttack(weakAttacker, strongDefender);

            // Should deal at least 1 damage even if defense is higher
            Assert.AreEqual(initialHP - 1, strongDefender.Stats.HP);
        }

        [Test]
        public void Battle_FireHasAdvantageOverElectric()
        {
            var fireSlime = new Slime("Fire", ElementType.Fire);
            var electricSlime = new Slime("Electric", ElementType.Electric);

            var advantage = ElementalAdvantage.GetMultiplier(ElementType.Fire, ElementType.Electric);

            Assert.Greater(advantage, 1.0f);
        }

        [Test]
        public void Battle_WaterHasAdvantageOverFire()
        {
            var waterSlime = new Slime("Water", ElementType.Water);
            var fireSlime = new Slime("Fire", ElementType.Fire);

            var advantage = ElementalAdvantage.GetMultiplier(ElementType.Water, ElementType.Fire);

            Assert.Greater(advantage, 1.0f);
        }

        [Test]
        public void Battle_ElectricHasAdvantageOverWater()
        {
            var electricSlime = new Slime("Electric", ElementType.Electric);
            var waterSlime = new Slime("Water", ElementType.Water);

            var advantage = ElementalAdvantage.GetMultiplier(ElementType.Electric, ElementType.Water);

            Assert.Greater(advantage, 1.0f);
        }

        [Test]
        public void Battle_NeutralHasNoAdvantage()
        {
            var neutralSlime = new Slime("Neutral", ElementType.Neutral);
            var fireSlime = new Slime("Fire", ElementType.Fire);

            var advantage = ElementalAdvantage.GetMultiplier(ElementType.Neutral, ElementType.Fire);

            Assert.AreEqual(1.0f, advantage);
        }

        [Test]
        public void Battle_ElementalAdvantageAffectsDamage()
        {
            var waterSlime = new Slime("Water", ElementType.Water);
            var fireSlime = new Slime("Fire", ElementType.Fire);

            var playerTeam = new List<Slime> { waterSlime };
            var enemyTeam = new List<Slime> { fireSlime };

            var battle = new Battle(playerTeam, enemyTeam);

            int initialHP = fireSlime.Stats.HP;
            int baseDamage = waterSlime.Stats.Attack - fireSlime.Stats.Defense;
            if (baseDamage < 1) baseDamage = 1;

            battle.ExecuteAttack(waterSlime, fireSlime);

            // Water has advantage over Fire, so damage should be increased
            int expectedDamage = (int)(baseDamage * 1.5f); // Assuming 1.5x multiplier
            Assert.AreEqual(initialHP - expectedDamage, fireSlime.Stats.HP);
        }

        [Test]
        public void Battle_IsOverWhenOneTeamIsDefeated()
        {
            var playerSlime = new Slime("Player", ElementType.Fire);
            var enemySlime = new Slime("Enemy", ElementType.Water);

            var playerTeam = new List<Slime> { playerSlime };
            var enemyTeam = new List<Slime> { enemySlime };

            var battle = new Battle(playerTeam, enemyTeam);

            Assert.IsFalse(battle.IsOver());

            // Reduce enemy HP to 0
            while (enemySlime.Stats.HP > 0)
            {
                battle.ExecuteAttack(playerSlime, enemySlime);
            }

            Assert.IsTrue(battle.IsOver());
        }

        [Test]
        public void Battle_CanDetermineWinner()
        {
            var playerSlime = new Slime("Player", ElementType.Fire);
            var enemySlime = new Slime("Enemy", ElementType.Water);

            var playerTeam = new List<Slime> { playerSlime };
            var enemyTeam = new List<Slime> { enemySlime };

            var battle = new Battle(playerTeam, enemyTeam);

            // Reduce enemy HP to 0
            while (enemySlime.Stats.HP > 0)
            {
                battle.ExecuteAttack(playerSlime, enemySlime);
            }

            var winner = battle.GetWinner();

            Assert.AreEqual(BattleTeam.Player, winner);
        }

        [Test]
        public void Battle_DeadSlimesAreRemovedFromTurnOrder()
        {
            var playerSlime = new Slime("Player", ElementType.Fire);
            var enemySlime = new Slime("Enemy", ElementType.Water);

            var playerTeam = new List<Slime> { playerSlime };
            var enemyTeam = new List<Slime> { enemySlime };

            var battle = new Battle(playerTeam, enemyTeam);

            var initialTurnOrderSize = battle.GetTurnOrder().Count;

            // Reduce enemy HP to 0
            while (enemySlime.Stats.HP > 0)
            {
                battle.ExecuteAttack(playerSlime, enemySlime);
            }

            var newTurnOrderSize = battle.GetTurnOrder().Count;

            Assert.AreEqual(initialTurnOrderSize - 1, newTurnOrderSize);
        }
    }
}
