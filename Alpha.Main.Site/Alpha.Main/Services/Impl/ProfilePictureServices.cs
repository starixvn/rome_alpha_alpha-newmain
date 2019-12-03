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
            Result.CreateErrorResult("Auto insert Profile Picture Failed!",ErrorCodeDefault);

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
                var command = new UpdateActorProfilePictureCommand(
                        param.ActorId,
                        param.ImageId,
                        param.Image,
                        param.IsDefault,
                        param.Mode,
                        param.IP);
                dynamicRepository.Execute(command);
                return Result.CreateErrorResult(command.ErrMsg, command.ErrCode);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return ErrorDefaultResult;
            }
        }
    }
}