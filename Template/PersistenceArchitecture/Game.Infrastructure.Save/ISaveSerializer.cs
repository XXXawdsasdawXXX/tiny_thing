namespace Game.Infrastructure.Save
{
    public interface ISaveSerializer
    {
        string Serialize<T>(T value);

        T Deserialize<T>(string json);
    }
}
