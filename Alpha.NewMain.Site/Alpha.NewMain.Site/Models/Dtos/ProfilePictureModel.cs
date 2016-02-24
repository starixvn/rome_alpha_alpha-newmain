namespace Alpha.NewMain.Site.Models.Dtos
{
    using System.Drawing;

    public class ProfilePictureModel
    {
        public int CustId { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public Image Avatar { get; set; }

        public byte[] BinaryAvatar { get; set; }
    }
}