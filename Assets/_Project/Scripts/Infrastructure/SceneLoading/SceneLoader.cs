using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.SceneLoading
{
    public class SceneLoader : ISceneLoader
    {
        public void Load(string sceneName)
        {
            Timing.RunCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator<float> LoadSceneCoroutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}