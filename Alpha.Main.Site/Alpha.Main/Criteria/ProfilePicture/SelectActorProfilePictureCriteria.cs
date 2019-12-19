namespace Alpha.Main.ProfilePicture.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class SelectActorProfilePictureCriteria : CriteriaBase
    {
        public SelectActorProfilePictureCriteria(int actorId)
        {
            ActorId = actorId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; private set; }

        public override string GetSettingKey() => "Age_CustomerSelProfilePicture";

        public override bool IsValid() => ActorId > 0;
    }
}
