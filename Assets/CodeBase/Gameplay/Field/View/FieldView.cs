using System;
using CodeBase.Gameplay.MVP;
using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class FieldView : ViewBase
    {
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(2,2);
        [SerializeField] private float _startYPosition = -4f;
        [SerializeField] private float _horizontalMargin = 2f;
        
        private void OnDrawGizmos()
        {
            // Margin
            Gizmos.color = Color.blue;
            var cameraSize = Camera.main.orthographicSize;
            float width = cameraSize * Camera.main.aspect;
            float bounds = width - _horizontalMargin * 2;
            Gizmos.DrawWireCube(
                transform.position,
                new Vector3(width - _horizontalMargin * 2, cameraSize * 2, 0));
            
            // Cells
            Gizmos.color = Color.green;
            
            float cellSize = bounds / _gridSize.x;
            
            for (int x = 0; x < _gridSize.x; x++)
            {
                float xPos = -bounds / 2 + cellSize / 2 + x * cellSize;
                for (int y = 0; y < _gridSize.y; y++)
                {
                    float yPos = _startYPosition + y * cellSize;
                    Gizmos.DrawWireCube(
                        new Vector3(xPos, yPos),
                        new Vector2(cellSize, cellSize));
                }
            }
        }
    }
}