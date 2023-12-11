using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class RotateTowards : MonoBehaviour
    {
        private const float RotationDuration = 0.1f;
        [CanBeNull] public Transform Target { get; set; }

        private void Update()
        {
            if (Target == null) return;
            
            var angle = Vector2.SignedAngle(Vector2.up, transform.position - Target.position);
            transform.DORotate(
                new Vector3(0, 0, angle),
                RotationDuration
            );
        }
    }
}