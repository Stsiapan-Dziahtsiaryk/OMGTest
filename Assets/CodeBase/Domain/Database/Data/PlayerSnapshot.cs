namespace CodeBase.Domain.Database.Data
{
    public readonly struct PlayerSnapshot : ISnapshot
    {
        public readonly int Level;

        public PlayerSnapshot(int level)
        {
            Level = level;
        }
    }
}