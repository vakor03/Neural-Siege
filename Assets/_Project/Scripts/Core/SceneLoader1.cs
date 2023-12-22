using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core
{
    public static class SceneLoader1
    {
        public static void LoadScene(SceneReference sceneReference)
        {
            SceneManager.LoadScene(sceneReference.Name);
        }
    }
}