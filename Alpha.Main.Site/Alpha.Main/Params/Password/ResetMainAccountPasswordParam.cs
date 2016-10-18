namespace Alpha.Main.Params.Password
{
    public class ResetMainAccountPasswordParam : ResetSubAccountPasswordParam
    {
        public bool IsCasinoDisabled { get; set; }

        public bool IsCasinoSync { get; set; }

        public bool IsResetMemberPassword { get; set; }
    }
}