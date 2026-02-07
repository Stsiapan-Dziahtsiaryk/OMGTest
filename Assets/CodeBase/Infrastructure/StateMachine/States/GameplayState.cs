using System;
using CodeBase.Infrastructure.Scenes;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public sealed class GameplayState : IState
    {
        private readonly ISceneService _scenes;

        public GameplayState(ISceneService scenes)
        {
            _scenes = scenes ?? throw new ArgumentNullException(nameof(scenes));
        }

        public void Enter()
        {
            _scenes
                .Load(SceneNames.Game)
                .AsUniTask()
                .Forget();
        }

        public void Exit()
        {
        }
    }
}
