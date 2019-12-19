namespace Alpha.Main.ProfilePicture.Params
{
    public class UpdateActorProfilePictureParam
    {
        public UpdateActorProfilePictureParam(int actorId, int mode, string ip)
        {
            ActorId = actorId;
            Mode = mode;
            IP = ip;
        }
        public int ActorId { get; }

        public int Mode { get; }

        public string IP { get; }
    }
}