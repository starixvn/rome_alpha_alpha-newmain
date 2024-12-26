using Alpha.Main.Site.ViewModels.WhiteLabel;
using MediatR;

namespace Alpha.Main.Site.Requests.WhiteLabel
{
    public class WhiteLabelSSORequest : IAsyncRequest<WhiteLabelSSOResponseViewModel>
    {
        public WhiteLabelSSORequest(string referenceId, string browserUserAgent, string userIp)
        {
            ReferenceId = referenceId;
            BrowserUserAgent = browserUserAgent;
            UserIp = userIp;
        }

        public string ReferenceId { get; }

        public string BrowserUserAgent { get; }

        public string UserIp { get; }
    }
}