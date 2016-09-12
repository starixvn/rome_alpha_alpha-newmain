namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;
    public class BaseSecurityCodeCommand : IRequest<Result>
    {
        //private ISecurityCodeSession securityCodeSession;

        //public BaseSecurityCodeCommand(ISecurityCodeSession securityCodeSession)
        //{
        //    this.securityCodeSession = securityCodeSession;
        //}

        //public string SecurityCodeToken
        //{
        //    get
        //    {
        //        return this.securityCodeSession.Language;
        //    }
        //}

        public string Language { get; set; }

        private string IP { get; set; }


    }
}