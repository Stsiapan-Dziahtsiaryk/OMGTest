using CodeBase.Domain.Database.Data;

namespace CodeBase.Domain.Database
{
    public interface ISheet
    {
        void Load();
    }
    
    public interface ISaveSheet<TData>
    : ISheet
    where TData : ISnapshot
    {
        TData Data { get; }
        
        void Save(TData data);
    }
}