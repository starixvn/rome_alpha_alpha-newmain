namespace Alpha.Main.Helpers
{
    public interface JsonHelper
    {
        T Deserialize<T>(string json);

        string Serialize(object data);
    }
}