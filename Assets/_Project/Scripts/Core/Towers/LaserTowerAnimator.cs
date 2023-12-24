using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class LaserTowerAnimator : MonoBehaviour
    {
        [SerializeField] private LaserTower laserTower;
        [SerializeField] private GameObject laser;
        [SerializeField] private float laserWidth = 0.1f;
        
        
        private void Start()
        {
            laserTower.OnAttacked += OnAttacked;
            laser.SetActive(false);
        }

        private void OnDestroy()
        {
            laserTower.OnAttacked -= OnAttacked;
        }

        private void OnAttacked()
        {
            laser.SetActive(true);
            
            laser.transform.DOScale(new Vector3(laserWidth,laserTower.Range*2,0), 0.1f).OnComplete(
                ()=> laser.transform.DOScale(new Vector3(0,laserTower.Range*2,0), 0.1f).OnComplete(
                    ()=> laser.SetActive(false)
                ));
        }
    }
}