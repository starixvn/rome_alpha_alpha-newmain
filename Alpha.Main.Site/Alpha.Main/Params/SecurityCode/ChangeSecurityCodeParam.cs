namespace Alpha.Main.Params.SecurityCode
{
    public class ChangeSecurityCodeParam : ResetSecurityCodeParam
    {
        public bool MustVerifyCurrentSecurityCode { get; set; }

        public string CurrentSecurityCode { get; set; }

        public string Salt { get; set; }
    }
}