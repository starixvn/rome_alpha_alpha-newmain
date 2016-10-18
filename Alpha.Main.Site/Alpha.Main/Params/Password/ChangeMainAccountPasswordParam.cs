namespace Alpha.Main.Params.Password
{
    public class ChangeMainAccountPasswordParam : ResetSubAccountPasswordParam
    {
        public bool IsCasinoDisabled { get; set; }

        public bool IsCasinoSync { get; set; }

        public string CurrentPassword { get; set; }

        public string NickName { get; set; }
    }
}
