using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Scenes
{
    public sealed class SceneService : ISceneService
    {
        public string ActiveSceneName => SceneManager.GetActiveScene().name;

        public async Task Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            if (mode == LoadSceneMode.Single && SceneManager.GetActiveScene().name == sceneName)
                return;

            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            if (operation == null)
                return;

            operation.allowSceneActivation = activateOnLoad;

            // Await until completed (or activation if allowed)
            while (!operation.isDone)
            {
                // If activation is disabled, stop at 0.9f to keep the scene prepared
                if (!activateOnLoad && operation.progress >= 0.9f)
                    break;
                await Task.Yield();
            }
        }

        public async Task Unload(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            if (!IsSceneLoaded(sceneName))
                return;

            var operation = SceneManager.UnloadSceneAsync(sceneName);
            if (operation == null)
                return;

            while (!operation.isDone)
            {
                await Task.Yield();
            }
        }

        public Task ReloadActive()
        {
            var current = SceneManager.GetActiveScene().name;
            // Load Single automatically unloads other loaded scenes
            return Load(current, LoadSceneMode.Single, true);
        }

        private static bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                    return true;
            }
            return false;
        }
    }
}
