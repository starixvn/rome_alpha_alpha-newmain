namespace Alpha.Main.ProfilePicture.Models
{
    using System;

    public class ActorProfilePictureModel
    {
        public ActorProfilePictureModel()
        {
        }

        public ActorProfilePictureModel(byte[] image, int imageId, DateTime? modified)
        {
            Image = image;
            ImageId = imageId;
            Modified = modified;
        }

        public byte[] Image { get; set; }

        public int ImageId { get; set; }

        public DateTime? Modified { get; set; }

        public string ImageBase64String => Image == null ? string.Empty : $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}";
    }
}
