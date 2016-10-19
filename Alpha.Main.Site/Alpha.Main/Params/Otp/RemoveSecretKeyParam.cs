namespace Alpha.Main.Params.Otp
{
    public class RemoveSecretKeyParam
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public int ActorId { get; set; }

        public string Language { get; set; }

        public bool IsDisableOtp { get; set; }

        public string IP { get; set; }
    }
}