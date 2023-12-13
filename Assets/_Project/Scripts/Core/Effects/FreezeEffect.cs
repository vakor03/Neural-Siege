namespace _Project.Scripts.Core.Effects
{
    public class FreezeEffect : Effect
    {
        public float FreezeMultiplier { get; set; }

        public FreezeEffect(float freezingMultiplier)
        {
            FreezeMultiplier = freezingMultiplier;
        }

        public override EnemyStats ApplyEffect(EnemyStats enemy)
        {
            enemy.speed *= FreezeMultiplier;
            return enemy;
        }

        public override void Reset()
        {
            // nothing to reset
        }
    }
}