namespace Alpha.Main.ProfilePicture.Services
{
    using Alpha.Main.Models;
    using Alpha.Main.ProfilePicture.Commands;
    using Alpha.Main.ProfilePicture.Criteria;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Params;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using System;

    public interface ProfilePictureServices
    {
        ActorProfilePictureModel GetActorProfilePicture(int custId);

        Result UpdateActorProfilePicture(UpdateActorProfilePictureParam param);
    }

    public class ProfilePictureServicesImpl : ProfilePictureServices
    {
        private const int ErrorCodeDefault = 100;
        private readonly IDynamicRepository dynamicRepository;
        private readonly Result ErrorDefaultResult =
            Result.CreateErrorResult("Auto insert Profile Picture Failed!", ErrorCodeDefault);

        public ProfilePictureServicesImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public ActorProfilePictureModel GetActorProfilePicture(int custId)
        {
            try
            {
                var model = dynamicRepository.Get<ActorProfilePictureModel>(new SelectActorProfilePictureCriteria(custId));
                return model ?? new ActorProfilePictureModel();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return new ActorProfilePictureModel();
            }
        }

        public Result UpdateActorProfilePicture(UpdateActorProfilePictureParam param)
        {
            try
            {
                var command = new UpdateActorProfilePictureMainServerCommand(
                        param.ActorId,
                        param.Mode,
                        param.IP);

                dynamicRepository.Execute(command);

                if (command.ErrCode.Equals(0))
                {
                    UpdateActorProfilePictureStandbyServerAsync(param);
                }

                return Result.CreateErrorResult(command.ErrMsg, command.ErrCode);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return ErrorDefaultResult;
            }
        }

        internal void UpdateActorProfilePictureStandbyServerAsync(UpdateActorProfilePictureParam param)
        {
            try
            {
                var command = new UpdateActorProfilePictureStandbyServerCommand(
                            param.ActorId,
                            param.Mode,
                            param.IP);

                dynamicRepository.Execute(command);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}