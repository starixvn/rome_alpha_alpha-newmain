namespace Alpha.Main.Site.Models.Views.CustomerProfile
{
    using System.Drawing;

    public class ProfilePictureViewModel
    {
        public int CustId { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public Image Avatar { get; set; }

        public byte[] BinaryAvatar { get; set; }
    }
}