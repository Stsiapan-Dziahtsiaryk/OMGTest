using System;

namespace CodeBase.Gameplay.Field
{
    public enum FieldState
    {
        Invalid = 0,
        Ready = 1,
        Building = 2,
        Normalize = 3, // divided into 2 states: Gravity and Matches 
        Gravity = 4,
        Matches = 5,
        Selection = 6,
        Rebuilding,
    }
}