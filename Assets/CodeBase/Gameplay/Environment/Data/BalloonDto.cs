using UnityEngine;

namespace CodeBase.Gameplay.Environment
{
    public readonly struct BalloonDto
    {
        public readonly Sprite Skin;
        public readonly Vector2 StartPoint;

        public BalloonDto(Sprite skin, Vector2 startPoint)
        {
            Skin = skin;
            StartPoint = startPoint;
        }
    }
}