namespace Alpha.Main.Models.ApiKey
{
    using System;

    public class ApiKeyInfo
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string ScopeIdList { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool IsExpired { get; set; }
    }
}
