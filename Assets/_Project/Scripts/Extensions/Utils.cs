using UnityEngine;

namespace _Project.Scripts.Extensions
{
    public static class Utils
    {
        public static Vector3 GetMouseToWorldPosition()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            return mousePosition;
        }

        public static void DrawGizmosCone(float coneAngle, float rayRange, float coneDirection, Transform transform,
            Color color)
        {
            float halfFOV = coneAngle / 2.0f;

            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

            Vector3 upRayDirection = upRayRotation * transform.right * rayRange;
            Vector3 downRayDirection = downRayRotation * transform.right * rayRange;

            Gizmos.color = color;
            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}