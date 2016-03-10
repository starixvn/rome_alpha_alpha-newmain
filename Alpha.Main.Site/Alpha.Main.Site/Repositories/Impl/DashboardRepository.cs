namespace Alpha.Main.Site.Repositories.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Alpha.Main.Site.Models.Dtos;
    using Criteria.CustomerProfile;
    using Fanex.Common;
    using Fanex.Data;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;

    public class DashboardRepository : IDashboardRepository
    {
        public List<LanguageModel> GetLanguages()
        {
            var languages = new List<LanguageModel>
            {
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
            avatar.IsDefaultAvatar = true;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());
                if (reader.Read())
                {
                    avatar.IsDefaultAvatar = false;
                    avatar.BinaryAvatar = Converter.ToByteArray(reader["image"]);
                }
            }

            return avatar;
        }

        public SiteMapNode GetSiteMapNode()
        {
            SiteMapNode rootNode = SiteMap.RootNode;
            
            return rootNode;        
                
                
                

            //var roleId = UserSession.Current.RoleId;
            //switch (roleId)
            //{
            //    case (int)UserTypes.Agent:
            //        return rootNode.ChildNodes[0];
            //    case (int)UserTypes.Master:
            //        return rootNode.ChildNodes[1];
            //    case (int)UserTypes.Super:
            //        return rootNode.ChildNodes[2];
            //    default:
            //        throw new BusinessException("Invalid role id for customer " + UserSession.Current.CustId + ": " + roleId);
            //}
        }
    }
}