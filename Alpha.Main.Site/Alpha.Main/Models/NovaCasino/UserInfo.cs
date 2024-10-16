namespace Alpha.Main.Models.NovaCasino
{
    public class UserInfo
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public int CurrencyId { get; set; }

        public int RoleId { get; set; }

        public User[] Upline { get; set; }
    }
}