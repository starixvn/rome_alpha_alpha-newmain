using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Alpha.Main.Configurations
{
    public static class NovaCasinoSettings
    {
        [ThreadStatic]
        private static NameValueCollection settings;

        internal static NameValueCollection Settings
        {
            get { return settings ?? ConfigurationManager.AppSettings; }

            set { settings = value; }
        }

        public static string ServiceUrl => Settings["NovaCasino:ServiceUrl"];

        public static string SecretKey => Settings["NovaCasino:SecretKey"];

        public static bool IsOfferedNovaCasino(string site, string level, string currency)
#pragma warning disable S1067 // Expressions should not be too complex
            => (NovaCasinoOfferedSites.Contains("*") || NovaCasinoOfferedSites.Contains(site))
            && (NovaCasinoOfferedCurrencies.Contains("*") || NovaCasinoOfferedCurrencies.Contains(currency))
            && DateTime.Parse(Settings[$"NovaCasino:{level}:OfferedDate"]) <= DateTime.Today;

#pragma warning restore S1067 // Expressions should not be too complex

        private static IEnumerable<string> NovaCasinoOfferedSites { get; } = new HashSet<string>(
            (Settings["NovaCasino:OfferedSites"] ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(site => site.Trim()), StringComparer.OrdinalIgnoreCase);

        private static IEnumerable<string> NovaCasinoOfferedCurrencies { get; } = new HashSet<string>(
           (Settings["NovaCasino:OfferedCurrencies"] ?? string.Empty)
               .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(site => site.Trim()), StringComparer.OrdinalIgnoreCase);
    }
}