using CodeBase.Infrastructure.Scenes;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.Infrastructure.AppRunner
{
    // Initializes app state machine on app start
    public sealed class AppStartup : IInitializable, IStartable
    {
        // private readonly AppStateMachine _machine;
        // private readonly BootstrapState _bootstrapState;
        // private readonly EnterState _enterState;
        // private readonly GameplayState _gameplayState;
        private readonly ISceneService _sceneService;
        
        public AppStartup(
            ISceneService sceneService)
        {
            _sceneService = sceneService;
            // _machine = machine;
            // _bootstrapState = bootstrapState;
            // _enterState = enterState;
            // _gameplayState = gameplayState;
        }

        public void Initialize()
        {
            Application.targetFrameRate = 60;
            
            // // Register states
            // _machine.Register(_bootstrapState);
            // _machine.Register(_enterState);
            // _machine.Register(_gameplayState);
        }
        
        public void Start()
        {
            // Enter first state
            // _machine.Enter<BootstrapState>();
            _sceneService.Load(SceneNames.Enter);
        }
    }
}
