namespace CodeBase.Infrastructure.StateMachine.States
{
    public interface IPayloadedState<in T> : IExitState
    {
        void Enter(T payload);
    }
}