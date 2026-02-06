using CodeBase.UI.StateMachine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public sealed class EnterState : IState
    {
        private readonly AppStateMachine _stateMachine;
        private readonly WindowStateMachine _windowStateMachine;

        public EnterState(AppStateMachine stateMachine, WindowStateMachine windowStateMachine)
        {
            _stateMachine = stateMachine;
            _windowStateMachine = windowStateMachine;
        }

        public void Enter()
        {
            // Show Menu window via UI state machine
            _windowStateMachine.Open(WindowType.Menu);
        }

        public void Exit()
        {
        }
    }
}
