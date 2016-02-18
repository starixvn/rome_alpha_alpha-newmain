namespace Alpha.NewMain.Site.Repositories.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using Alpha.NewMain.Site.Models.Dtos;
    using Criteria.CustomerProfile;
    using Fanex.Common;
    using Fanex.Data;
    using Models.Views.CustomerProfile;

    public class DashboardRepository : IDashboardRepository
    {
        public List<LanguageModel> GetLanguages()
        {
            var languages = new List<LanguageModel> {
                new LanguageModel
                {
                    LanguageCode = "en-US",
                    LanguageName = "English"
                },
                new LanguageModel
                {
                    LanguageCode = "zh-TW",
                    LanguageName = "繁體中文"
                },
                new LanguageModel
                {
                    LanguageCode = "zh-CN",
                    LanguageName = "简体中文"
                },
                new LanguageModel
                {
                    LanguageCode = "ja-JP",
                    LanguageName = "日本語"
                },
                new LanguageModel
                {
                    LanguageCode = "th-TH",
                    LanguageName = "ภาษาไทย"
                },
                new LanguageModel
                {
                    LanguageCode = "ko-KR",
                    LanguageName = "한국어"
                },
                new LanguageModel
                {
                    LanguageCode = "vi-VN",
                    LanguageName = "Tiếng Việt"
                },
            };

            return languages;
        }

        public ProfilePictureModel GetAvatar(ViewProfileCriteria criteria)
        {
            var avatar = new ProfilePictureModel();
            var dataSet = new DataSet();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var paramsOuput = criteria.GetSpParams();
                dataSet = db.ExecuteDataSet(paramsOuput);
            }

            var isDefaultAvatar = dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0;

            avatar.IsDefaultAvatar = isDefaultAvatar;
            if (!isDefaultAvatar)
            {
                avatar.BinaryAvatar = Converter.ToByteArray(dataSet.Tables[0].Rows[0]["image"]);
            }

            return avatar;
        }
    }
}