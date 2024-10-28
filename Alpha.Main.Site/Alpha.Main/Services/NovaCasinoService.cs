using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Alpha.Main.AppResources;
using Alpha.Main.Configurations;
using Alpha.Main.Models.NovaCasino;
using Fanex.Logging;
using Newtonsoft.Json;

namespace Alpha.Main.Services
{
    public interface INovaCasinoService
    {
        Task<NovaCasinoSSOResult> SSO(NovaCasinoSSORequest request);
    }

    public class NovaCasinoService : INovaCasinoService
    {
        private readonly ILogger logger;

        public NovaCasinoService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<NovaCasinoSSOResult> SSO(NovaCasinoSSORequest request)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var message = JsonConvert.SerializeObject(request);
                    httpClient.DefaultRequestHeaders.Add("Signature", GenerateSignature(message, NovaCasinoSettings.SecretKey));
                    using (var stringContent = new StringContent(message, Encoding.UTF8, "application/json"))
                    {
#pragma warning disable S4005 // "System.Uri" arguments should be used instead of strings
                        using (var httpResponseMessage = await httpClient.PostAsync($"{NovaCasinoSettings.ServiceUrl}/api/NovaCasino/SingleSignOn", stringContent).ConfigureAwait(false))
#pragma warning restore S4005 // "System.Uri" arguments should be used instead of strings
                        {
                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                                var response = JsonConvert.DeserializeObject<NovaCasinoSSOResponse<SSOSuccess>>(responseString);
                                return new NovaCasinoSSOResult { LaunchUrl = response.Data.LaunchUrl, CustIds = response.Data.CustIds, Message = response.ErrorMessage };
                            }

                            if (httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
                            {
                                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                                var response = JsonConvert.DeserializeObject<NovaCasinoSSOResponse<UnderMaintenance>>(responseString);
                                return new NovaCasinoSSOResult
                                {
                                    Code = (int)httpResponseMessage.StatusCode,
                                    Message = string.Format(Resources.SystemUnderMaintenance, response.Data.StartTime, response.Data.EndTime)
                                };
                            }

                            await logger.ErrorAsync(await httpResponseMessage.Content.ReadAsStringAsync());

                            return new NovaCasinoSSOResult
                            {
                                Code = (int)HttpStatusCode.InternalServerError,
                                Message = Resources.InternalServerError
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);

                return new NovaCasinoSSOResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = Resources.InternalServerError
                };
            }
        }

        private static string GenerateSignature(string message, string secretKey)
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