using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class EnemiesController : MonoBehaviour
    {
        [field: SerializeField] public LayerMask EnemyLayerMask { get; private set; }
        public static EnemiesController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}