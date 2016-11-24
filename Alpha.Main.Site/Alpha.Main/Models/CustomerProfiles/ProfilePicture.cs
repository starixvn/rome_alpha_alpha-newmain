namespace Alpha.Main.Models.CustomerProfiles
{
    public class ProfilePicture
    {
        public ProfilePicture(int custId, bool useDefaultAvatar, byte[] imageData = null)
        {
            CustId = custId;
            UseDefaultAvatar = useDefaultAvatar;
            ImageData = imageData;
        }

        public int CustId { get; }

        public bool UseDefaultAvatar { get; }

        public byte[] ImageData { get; }
    }
}