namespace Alpha.Main.Models.SecurityCode
{
    using Alpha.Main.Models.Enumerations;

    public class SecurityCode
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string Language { get; set; }

        public string VirtualDirectory { get; set; }
    }
}