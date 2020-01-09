namespace Alpha.Main.ProfilePicture.Services
{
    using Alpha.Main.Helpers.Impl;
    using Alpha.Main.Models;
    using Alpha.Main.ProfilePicture.Commands;
    using Alpha.Main.ProfilePicture.Criteria;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Params;
    using Fanex.Data.Repository;
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
        private readonly ExceptionHandlerWrapper exceptionHandlerWrapper;
        private readonly Result ErrorDefaultResult =
            Result.CreateErrorResult("Auto insert Profile Picture Failed!", ErrorCodeDefault);

        public ProfilePictureServicesImpl(IDynamicRepository dynamicRepository, ExceptionHandlerWrapper exceptionHandlerWrapper)
        {
            this.dynamicRepository = dynamicRepository;
            this.exceptionHandlerWrapper = exceptionHandlerWrapper;
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
                exceptionHandlerWrapper.Handle(ex);
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
                    UpdateActorProfilePictureStandbyServer(param);
                }

                return Result.CreateErrorResult(command.ErrMsg, command.ErrCode);
            }
            catch (Exception ex)
            {
                exceptionHandlerWrapper.Handle(ex);
                return ErrorDefaultResult;
            }
        }

        internal void UpdateActorProfilePictureStandbyServer(UpdateActorProfilePictureParam param)
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
                exceptionHandlerWrapper.Handle(ex);
            }
        }
    }
}