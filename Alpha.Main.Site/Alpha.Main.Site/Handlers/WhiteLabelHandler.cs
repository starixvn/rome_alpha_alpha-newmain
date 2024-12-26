using Alpha.Main.Services;
using Alpha.Main.Site.Requests.WhiteLabel;
using Alpha.Main.Site.ViewModels.WhiteLabel;
using MediatR;
using System.Net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Alpha.Main.Configurations;
using AlphaPlus.User.Models;
using Newtonsoft.Json;
using Sentry.Protocol;
using System.Text;
using System.Web.Mvc;
using Alpha.Main.AppResources;

namespace Alpha.Main.Site.Handlers
{
    public class WhiteLabelHandler : IAsyncRequestHandler<WhiteLabelSSORequest, WhiteLabelSSOResponseViewModel>
    {
        private readonly HttpClient httpClient;
        private readonly WhiteLabelService whiteLabelService;

        public WhiteLabelHandler(WhiteLabelService whiteLabelService)
        {
            httpClient = CreateHttpClient();
            this.whiteLabelService = whiteLabelService;
        }

        public async Task<WhiteLabelSSOResponseViewModel> Handle(WhiteLabelSSORequest message)
        {
            var isOffered = await whiteLabelService.IsOfferedWhiteLabel(int.Parse(message.ReferenceId));
            if (isOffered)
            {
                var ssoResponse = await SendSSORequest(message);
                return new WhiteLabelSSOResponseViewModel { ReturnUrl = ssoResponse.ReturnUrl };
            }

            return new WhiteLabelSSOResponseViewModel
            {
                Code = 1,
                Message = Resources.NotSetupWhiteLabelMessage,
            };
        }

        private async Task<WhiteLabelSSOResponse> SendSSORequest(WhiteLabelSSORequest request)
        {
            using (var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
            {
                using (var response = await httpClient.PostAsync("Signin", jsonContent))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonResponse = await response.Content.ReadAsAsync<WhiteLabelSSOResponse>();
                    return jsonResponse;
                }
            }
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = AppSettings.WhiteLabelBackOfficeApiUri,
                Timeout = TimeSpan.FromSeconds(15),
            };
        }
    }
}