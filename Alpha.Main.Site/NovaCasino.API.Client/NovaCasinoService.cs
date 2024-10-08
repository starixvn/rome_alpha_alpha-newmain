using NovaCasino.API.Client.Models;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using NovaCasino.API.Client.Configurations;

namespace NovaCasino.API.Client
{
    public interface INovaCasinoService
    {
        Task<NovaCasinoSSOResponse> SSO(NovaCasinoSSORequest request);
    }

    public class NovaCasinoService : INovaCasinoService
    {
        public async Task<NovaCasinoSSOResponse> SSO(NovaCasinoSSORequest request)
        {
            using (var httpClient = new HttpClient())
            {
                var message = JsonConvert.SerializeObject(request);
                httpClient.DefaultRequestHeaders.Add("Signature", GenerateSignature(message, NovaCasinoSettings.SecretKey));
                using (var stringContent = new StringContent(message, Encoding.UTF8, "application/json"))
                {
                    using (var httpResponseMessage = await httpClient.PostAsync($"{NovaCasinoSettings.ServiceUrl}/api/NovaCasino/SingleSignOn", stringContent).ConfigureAwait(false))
                    {
                        httpResponseMessage.EnsureSuccessStatusCode();
                        var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<NovaCasinoSSOResponse>(responseString);
                    }
                }
            }
        }

        private string GenerateSignature(string message, string secretKey)
        {
            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashMessage);
            }
        }
    }
}