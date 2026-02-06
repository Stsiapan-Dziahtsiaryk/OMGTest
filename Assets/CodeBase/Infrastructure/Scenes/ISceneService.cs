using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Scenes
{
    public interface ISceneService
    {
        string ActiveSceneName { get; }

        Task Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, bool activateOnLoad = true);

        Task Unload(string sceneName);

        Task ReloadActive();
    }
}
