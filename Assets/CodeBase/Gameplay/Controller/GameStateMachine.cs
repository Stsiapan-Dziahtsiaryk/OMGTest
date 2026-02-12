using System;
using CodeBase.Domain.Configs;
using CodeBase.Domain.Database;
using CodeBase.Domain.Database.Data;
using CodeBase.Domain.Database.Sheets;
using CodeBase.Gameplay.Environment;
using CodeBase.Gameplay.Field.Config;
using CodeBase.Gameplay.Level;
using R3;

namespace CodeBase.Gameplay.Controller
{
    public class GameStateMachine : IDisposable
    {
        private readonly LevelBuilder _builder;
        private readonly GameSettings _settings;
        private readonly LevelConfigLoader _configLoader;
        private readonly DatabaseService _database;
        
        private readonly ReactiveProperty<GameState> _state;

        private int _currentLevel = 0;
        private LevelData _currentLevelData;
        
        public GameStateMachine(
            LevelBuilder builder,
            GameSettings settings, 
            DatabaseService database, 
            LevelConfigLoader configLoader)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _database = database ??  throw new ArgumentNullException(nameof(database));
            _configLoader = configLoader ?? throw new ArgumentNullException(nameof(configLoader));
            _state = new ReactiveProperty<GameState>(GameState.Invalid);
        }

        public void HandleNewGame()
        {
            var playerSnapshot = _database.GetSheet<PlayerSheet>().Data;
            
            _state.Value = GameState.Play;
            _currentLevel = playerSnapshot.Level;
            _builder.Build(GetLevel());
        }

        public void RestartLevel()
        {
            _state.Value = GameState.Restart;
            
            _builder.Rebuild(_currentLevelData);
        }
        
        public void NextLevel()
        {
            _state.Value = GameState.End;
            
            _currentLevel++;
            int current = (_currentLevel) % _settings.LevelAmount;
            
            _currentLevelData = _configLoader.GetData(current);
            _builder.Rebuild(_currentLevelData);
            
            _database.GetSheet<PlayerSheet>().Save(new PlayerSnapshot(_currentLevel));
            _database.GetSheet<LevelSheet>().Save(new LevelSnapshot(false, default));
        }

        public void Dispose()
        {
        }

        private LevelData GetLevel()
        {
            var levelSnapshot = _database.GetSheet<LevelSheet>().Data;
            int current = (_currentLevel) % _settings.LevelAmount;

            if (levelSnapshot.IsInterrupted)
            {
                _currentLevelData = _configLoader.GetData(current);
                return levelSnapshot.Level;
            }
            else
            {
                return _configLoader.GetData(current);
            }
        }
    }
}