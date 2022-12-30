namespace Alpha.Main.Services.Impl
{
    using Alpha.NetThrottling;
    using Alpha.NetThrottling.Models;
    using Alpha.Web.Configurations;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Web;

    public interface INetThrottlingService
    {
        ThrottlePolicy ThrottlePolicy { get; }

        int GetBlockRemainingSeconds();

        bool IsBlacklist();

        void Throttle();
    }

    public class NetThrottlingService : INetThrottlingService
    {
        private readonly IThrottleSetting throttleSetting;
        private readonly INetThrottler netThrottler;

        public ThrottlePolicy ThrottlePolicy { get; }

        public NetThrottlingService(INetThrottler netThrottler, IThrottleSetting throttleSetting)
        {
            this.netThrottler = netThrottler;
            this.throttleSetting = throttleSetting;
            ThrottlePolicy = new ThrottlePolicy(throttleSetting.GetKey("Main"));
        }

        public int GetBlockRemainingSeconds()
        {
            var timeSpan = netThrottler.GetBlockRemainingTime(ThrottlePolicy, GetCacheIdentity());
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }

        public bool IsBlacklist()
        {
            return netThrottler.IsBlacklist(GetCacheIdentity());
        }

        public void Throttle()
        {
            var cacheIdentity = GetCacheIdentity();
            if (netThrottler.IsBlacklist(cacheIdentity))
            {
                return;
            }

            netThrottler.Throttle(ThrottlePolicy, cacheIdentity, true);
        }

        private CacheIdentity GetCacheIdentity()
        {
            return CacheIdentity.From(
                ThrottlePolicy.Name,
                AppSettings.SiteLabel,
                new[] { HttpContext.Current.Request.Path, UserSession.Current.UserName, UserSession.Current.SubUserName });
        }
    }
}
