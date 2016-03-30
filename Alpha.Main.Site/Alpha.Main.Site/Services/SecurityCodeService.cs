namespace Alpha.Main.Site.Services
{
    using Fanex.Razorbill.Security.Entities;

    public interface SecurityCodeService
    {
        bool IsActive { get; }

        bool IsRequireSecurityCode { get; }

        bool IsInternal { get; }

        SecurityCodeStatus SecurityCodeResult { get; }

        bool IsIgnoreSecurityCode(string url);

        bool IsForceUseSecurityCode(string site);
    }
}
