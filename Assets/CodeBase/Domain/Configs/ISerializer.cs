namespace CodeBase.Domain.Configs
{
    public interface ISerializer
    {
        string Serialize(object @object);
        T Deserialize<T>(string jsonString);
    }
}