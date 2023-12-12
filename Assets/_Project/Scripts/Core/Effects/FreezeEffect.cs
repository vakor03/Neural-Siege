namespace _Project.Scripts.Core.Effects
{
    public class FreezeEffect : Effect
    {
        private readonly float _speedMultiplier;

        public FreezeEffect(float freezingMultiplier)
        {
            _speedMultiplier = freezingMultiplier;
        }

        public override EnemyStats ApplyEffect(EnemyStats enemy)
        {
            enemy.speed *= _speedMultiplier;
            return enemy;
        }

        public override void Reset()
        {
            // nothing to reset
        }
    }
}