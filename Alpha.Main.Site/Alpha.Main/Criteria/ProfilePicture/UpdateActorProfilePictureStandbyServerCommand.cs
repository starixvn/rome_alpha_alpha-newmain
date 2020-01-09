namespace Alpha.Main.ProfilePicture.Commands
{
    public class UpdateActorProfilePictureStandbyServerCommand : UpdateActorProfilePictureCommandBase
    {
        public UpdateActorProfilePictureStandbyServerCommand(int actorId, int mode, string ip)
            : base(actorId, mode, ip, false)
        {
        }

        public override string GetSettingKey() => "Age_CustomerUpdProfilePicture_Standby";
    }
}