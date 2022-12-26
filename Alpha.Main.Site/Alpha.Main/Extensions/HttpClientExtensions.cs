
namespace Alpha.Main.Extensions
{
    using Alpha.Main.Models.ApiKey;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, string requestUri)
        {
            var uri = new Uri(httpClient.BaseAddress, requestUri);
            var response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<string> PostAsStringAsync(this HttpClient httpClient, string requestUri, object value = null)
        {
            var json = value != null ? JsonConvert.SerializeObject(value) : string.Empty;
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(httpClient.BaseAddress, requestUri),
                Content = content
            };
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, object value = null)
        {
            var content = await PostAsStringAsync(httpClient, requestUri, value);
            return JsonConvert.DeserializeObject<T>(content);
        }

        //public static async Task<T> PutAsStringAsync<T>(this HttpClient httpClient, string requestUri, object value = null)
        //{
        //    var json = value != null ? JsonConvert.SerializeObject(value) : string.Empty;
        //    var contentString = new StringContent(json, Encoding.UTF8, "application/json");
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Put,
        //        RequestUri = new Uri(httpClient.BaseAddress, requestUri),
        //        Content = contentString
        //    };
        //    var response = await httpClient.SendAsync(request);
        //    response.EnsureSuccessStatusCode();
        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(content);
        //}

        //public static async Task<T> DeleteAsStringAsync<T>(this HttpClient httpClient, string requestUri)
        //{
        //    var uri = new Uri(httpClient.BaseAddress, requestUri);
        //    var response = await httpClient.DeleteAsync(uri);
        //    response.EnsureSuccessStatusCode();
        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(content);
        //}

        public static async Task PutAsStringAsync(this HttpClient httpClient, string requestUri, object value = null)
        {
            var json = value != null ? JsonConvert.SerializeObject(value) : string.Empty;
            var contentString = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(httpClient.BaseAddress, requestUri),
                Content = contentString
            };
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public static async Task DeleteAsStringAsync(this HttpClient httpClient, string requestUri)
        {
            var uri = new Uri(httpClient.BaseAddress, requestUri);
            var response = await httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
        }
    }
}
