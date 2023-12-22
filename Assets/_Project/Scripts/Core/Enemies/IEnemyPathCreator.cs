using System;

namespace _Project.Scripts.Core.Enemies
{
    public interface IEnemyPathCreator
    {
        event Action OnPathCreated;
        void CreatePath();
    }
}