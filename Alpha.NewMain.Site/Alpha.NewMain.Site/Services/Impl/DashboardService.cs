namespace Alpha.NewMain.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Alpha.NewMain.Site.Repositories;
    using Criteria.CustomerProfile;
    using Extensions;
    using Models.Dtos;
    using Models.Requests.CustomerProfile;
    using Models.Views.CustomerProfile;
    using Sunplus.Agent.Utilities;

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        private readonly IFileUploadService _fileUploadService;

        public DashboardService(
            IDashboardRepository repository,
            IFileUploadService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        public List<LanguageModel> GetLanguages()
        {
            return _repository.GetLanguages();
        }

        public ViewProfileViewModel GetProfile(ViewProfileQuery message)
        {
            var avatar = GetAvatar(new ViewProfileCriteria
            {
                CustId = message.CustId
            });

            var result = new ViewProfileViewModel
            {
                IsDefaultAvatar = avatar.IsDefaultAvatar,
                EmbedImage = GenerateImage(avatar)
            };

            return result;
        }

        public ProfilePictureModel GetAvatar(ViewProfileCriteria criteria)
        {
            var avatar = _repository.GetAvatar(criteria);
            if (avatar.IsDefaultAvatar)
            {
                avatar.BinaryAvatar = _fileUploadService.GetDefaultAvatar();
            }

            SessionExtension.IsDefaultAvatar = avatar.IsDefaultAvatar;
            SessionExtension.BinaryAvatar = avatar.BinaryAvatar;
            return avatar;
        }

        private string GenerateImage(ProfilePictureModel avatar)
        {
            try
            {
                byte[] binaryAvatar = avatar.BinaryAvatar;
                if (binaryAvatar != null)
                {
                    return "data:image/jpeg;base64," + Convert.ToBase64String(binaryAvatar);
                }
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }

            return "data:image/jpeg;base64," + string.Empty;
        }
    }
}