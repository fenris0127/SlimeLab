namespace SlimeLab.Core
{
    public class SlimeStats
    {
        public int HP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Speed { get; private set; }

        public SlimeStats(int hp, int attack, int defense, int speed)
        {
            HP = hp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
        }

        public void BoostStats(int hpBoost, int attackBoost, int defenseBoost, int speedBoost)
        {
            HP += hpBoost;
            Attack += attackBoost;
            Defense += defenseBoost;
            Speed += speedBoost;
        }
    }
}
