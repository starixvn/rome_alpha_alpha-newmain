namespace Alpha.Main.ProfilePicture.Commands
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateActorProfilePictureCommand : NonQueryCommand
    {
        public UpdateActorProfilePictureCommand(
            int actorId,
            int? imageId,
            byte[] image,
            bool isDefault,
            int mode,
            string ip)
        {
            ActorId = actorId;
            if(imageId != null && imageId < 0)
            {
                ImageId = null;
                Image = image;
            }
            else
            {
                ImageId = imageId;
                Image = null;
            }
          
            IsDefault = isDefault;
            Mode = mode;
            IP = ip;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; }

        public int? ImageId { get;}

        public byte[] Image { get; }

        public bool IsDefault { get; }

        public bool IsMainServer => true;

        public int Mode { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerUpdProfilePicture";

        public override bool IsValid() => ActorId > 0;
    }
}