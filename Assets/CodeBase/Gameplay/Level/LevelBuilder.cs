using System;
using CodeBase.Gameplay.Field;
using CodeBase.Gameplay.Field.Config;
using UnityEngine;

namespace CodeBase.Gameplay.Level
{
    public class LevelBuilder
    {
        private readonly GameSettings _settings;
        private readonly FieldModel _fieldModel; // ToDo: remove
        private readonly Camera _camera;

        public LevelBuilder(
            GameSettings settings,
            FieldModel fieldModel)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _fieldModel = fieldModel ?? throw new ArgumentNullException(nameof(fieldModel));
            _camera = Camera.main;
        }

        public void Rebuild(LevelConfig config)
        {
            _fieldModel.ChangeState(FieldState.Rebuilding);
            Build(config);
        }
        
        public void Rebuild(LevelData config)
        {
            _fieldModel.ChangeState(FieldState.Rebuilding);
            Build(config);
        }
        
        public void Build(LevelConfig config)
        {
            var cameraSize = _camera.orthographicSize;
            float width = cameraSize * _camera.aspect;
            float bounds = width - _settings.HorizontalMargin * 2;

            float size = bounds / config.GridSize.x;
            float scale = size / _settings.ReferenceSizeBlock;

            Cell[,] gridArray = new Cell[config.GridSize.x, config.GridSize.y];
            
            for (int y = 0; y < config.GridSize.y; y++)
            {
                float yPos = _settings.StartYPosition + y * size;
                for (int x = 0; x < config.GridSize.x; x++)
                {
                    int index = y * config.GridSize.x + x;
                    Vector2 position = new Vector2((-bounds + size) / 2 + x * size, yPos);
                    Cell cell = new Cell(config.Grid[index], position);
                    gridArray[x, y] = cell;
                }
            }
            
            _fieldModel.Initialize(scale, gridArray);
        }

        public void Build(LevelData level)
        {
            var cameraSize = _camera.orthographicSize;
            float width = cameraSize * _camera.aspect;
            float bounds = width - _settings.HorizontalMargin * 2;

            float size = bounds / level.GridSize.x;
            float scale = size / _settings.ReferenceSizeBlock;

            Cell[,] gridArray = new Cell[level.GridSize.x, level.GridSize.y];
            
            for (int y = 0; y < level.GridSize.y; y++)
            {
                float yPos = _settings.StartYPosition + y * size;
                for (int x = 0; x < level.GridSize.x; x++)
                {
                    int index = y * level.GridSize.x + x;
                    Vector2 position = new Vector2((-bounds + size) / 2 + x * size, yPos);
                    Cell cell = new Cell(level.Grid[index], position);
                    gridArray[x, y] = cell;
                }
            }
            
            _fieldModel.Initialize(scale, gridArray);
        }
    }
}