using UnityEngine;

namespace _Project.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public void TakeDamage(float damage)
        {
            Debug.Log($"Enemy took {damage} damage");
        }
    }
}