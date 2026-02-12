using CodeBase.Gameplay.Field.Config;

namespace CodeBase.Domain.Database.Data
{
    public readonly struct LevelSnapshot :  ISnapshot
    {
        public readonly bool IsInterrupted;
        public readonly LevelData Level;

        public LevelSnapshot(bool isInterrupted, LevelData level)
        {
            IsInterrupted = isInterrupted;
            Level = level;
        }
    }
}