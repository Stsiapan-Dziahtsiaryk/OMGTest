using System.Threading.Tasks;
using CodeBase.Infrastructure.Scenes;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public sealed class BootstrapState : IState
    {
        private readonly AppStateMachine _stateMachine;
        private readonly ISceneService _scenes;

        public BootstrapState(AppStateMachine stateMachine, ISceneService scenes)
        {
            _stateMachine = stateMachine;
            _scenes = scenes;
        }

        public void Enter()
        {
            _ = EnterAsync();
        }

        public void Exit()
        {
        }

        private async Task EnterAsync()
        {
            // Switch from Bootstrap Scene to Enter Scene, then move to EnterState
            await _scenes.Load(SceneNames.Enter, LoadSceneMode.Single);
            _stateMachine.Enter<EnterState>();
        }
    }
}
