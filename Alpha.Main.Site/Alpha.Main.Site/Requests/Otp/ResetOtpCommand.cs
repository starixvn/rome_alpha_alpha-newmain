namespace Alpha.Main.Site.Requests.Otp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Alpha.Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Security;

    public class ResetOtpCommand : IRequest<ResetOtpViewModel>
    {
        private readonly IUserSession userSession;

        public ResetOtpCommand() : this(null)
        {
        }

        public ResetOtpCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string Username { get; set; }

        public int ActorId
        {
            get
            {
                return this.userSession.UserId;
            }
        }

        public int SubActorId
        {
            get
            {
                return this.userSession.SubAccId;
            }
        }

        public string Language
        {
            get
            {
                return this.userSession.LangKey;
            }
        }

        public string IP
        {
            get
            {
                return this.userSession.UserLoginIp;
            }
        }
    }
}