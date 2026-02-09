using System;
using CodeBase.Gameplay.Level;
using R3;

namespace CodeBase.Gameplay.Controller
{
    public class GameStateMachine
    {
        private readonly LevelBuilder _builder;
        private readonly GameSettings _settings;

        private readonly ReactiveProperty<GameState> _state;

        private int _currentLevel = 0;
        
        public GameStateMachine(
            LevelBuilder builder,
            GameSettings settings)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _state = new ReactiveProperty<GameState>(GameState.Invalid);
        }

        public void HandleNewGame()
        {
            _state.Value = GameState.Play;
            _builder.Build(_settings.LevelConfigs[_currentLevel]);
        }

        public void RestartLevel()
        {
            _state.Value = GameState.Restart;
            _builder.Rebuild(_settings.LevelConfigs[_currentLevel]);
        }
        
        public void NextLevel()
        {
            _state.Value = GameState.End;
            int old = _currentLevel;
            _currentLevel = (old + 1) % _settings.LevelConfigs.Length;
            _builder.Rebuild(_settings.LevelConfigs[_currentLevel]);
        }
    }
}