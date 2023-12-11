using UnityEngine;

namespace _Project.Scripts
{
    public static class Utils
    {
        public static Vector3 GetMouseToWorldPosition()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            return mousePosition;
        }
    }
}