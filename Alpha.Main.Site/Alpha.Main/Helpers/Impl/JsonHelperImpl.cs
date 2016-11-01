namespace Alpha.Main.Helpers.Impl
{
    using Newtonsoft.Json;

    public class JsonHelperImpl : JsonHelper
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}