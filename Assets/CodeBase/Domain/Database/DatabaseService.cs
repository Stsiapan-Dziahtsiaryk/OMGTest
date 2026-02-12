using System;
using System.Collections.Generic;
using CodeBase.Domain.Database.Sheets;
using VContainer.Unity;

namespace CodeBase.Domain.Database
{
    public class DatabaseService : IInitializable, IDisposable
    {
        private readonly Dictionary<Type, ISheet> _sheets;

        public DatabaseService()
        {
            _sheets = new Dictionary<Type, ISheet>()
            {
                [typeof(PlayerSheet)] = new PlayerSheet(),
                [typeof(LevelSheet)] = new LevelSheet(),
            };
        }
        
        public void Initialize()
        {
            foreach (var keyValuePair in _sheets)
            {
                keyValuePair.Value.Load();
            }
        }

        public TSheet GetSheet<TSheet>()
        where TSheet : class, ISheet
        {
            return _sheets[typeof(TSheet)] as TSheet;
        }
        
        public void Dispose()
        {
            
        }
    }
}