using System;

namespace CodeBase.Gameplay.Field
{
    [Flags]
    public enum FieldState
    {
        Invalid = 0,
        Ready = 1 << 1,
        Building = 1 << 2,
        Normalize = 1 << 3,
        Rebuilding = 1 << 4,
    }
}