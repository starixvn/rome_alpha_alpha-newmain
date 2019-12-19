namespace Alpha.Main.ProfilePicture.Commands
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateActorProfilePictureCommand : NonQueryCommand
    {
        public UpdateActorProfilePictureCommand(int actorId, int mode, string ip)
        {
            ActorId = actorId;
            IsDefault = true;
            Mode = mode;
            IP = ip;
            IsMainServer = true;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; }

        public bool IsDefault { get; }

        public bool IsMainServer { get; }

        public int Mode { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerUpdProfilePicture";

        public override bool IsValid() => ActorId > 0;
    }
}