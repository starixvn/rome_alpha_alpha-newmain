namespace Alpha.Main.Site.Models.CustomerProfiles
{
    using System.Drawing;

    public class ProfilePicture
    {
        public int CustId { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public Image Avatar { get; set; }

        public byte[] BinaryAvatar { get; set; }
    }
}