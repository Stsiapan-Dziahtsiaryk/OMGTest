using CodeBase.Infrastructure.StateMachine.States;
using VContainer.Unity;

namespace CodeBase.Infrastructure.StateMachine
{
    // Initializes app state machine on app start
    public sealed class AppStartup : IInitializable, IStartable
    {
        private readonly AppStateMachine _machine;
        private readonly BootstrapState _bootstrapState;
        private readonly EnterState _enterState;
        private readonly GameplayState _gameplayState;

        public AppStartup(
            AppStateMachine machine,
            BootstrapState bootstrapState,
            EnterState enterState,
            GameplayState gameplayState)
        {
            _machine = machine;
            _bootstrapState = bootstrapState;
            _enterState = enterState;
            _gameplayState = gameplayState;
        }

        public void Initialize()
        {
            // Register states
            _machine.Register(_bootstrapState);
            _machine.Register(_enterState);
            _machine.Register(_gameplayState);
        }
        
        public void Start()
        {
            // Enter first state
            _machine.Enter<BootstrapState>();
        }
    }
}
