namespace _Project.Scripts.Core.Effects
{
    public abstract class Effect
    {
        public virtual EnemyStats ApplyEffect(EnemyStats enemy)
        {
            return enemy;
        }

        public virtual void Reset()
        {
        }

        public virtual void Update(Enemy enemy, float deltaTime)
        {
        }
    }
}