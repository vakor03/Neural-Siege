using UnityEngine;

namespace _Project.Scripts
{
    public class FollowCursor : MonoBehaviour
    {
        private void Update()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }
}