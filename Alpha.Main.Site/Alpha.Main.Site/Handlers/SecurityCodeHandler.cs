namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.SecurityCode.Web;
    using Alpha.Web.Models;
    using AppResources;
    using Fanex.Cryptography.Aes;
    using Main.Models;
    using MediatR;
    using Models.Request.SecurityCode;
    using Params.SecurityCode;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using System;
    using ViewModels.Security;
    using Web.Helpers;
    using CustomResult = Alpha.Main.Models.Result;

    public class SecurityCodeHandler :
        IRequestHandler<ForceChangeSecurityCodeQuery, ForceChangeSecurityCodeViewModel>,
        IRequestHandler<SecurityCodeQuery, SecurityCodeViewModel>,
        IRequestHandler<ChangeSecurityCodeFirstLoginCommand, CustomResult>,
        IRequestHandler<ChangeSecurityCodeByForcingCommand, CustomResult>,
        IRequestHandler<ChangeUserOwnSecurityCodeCommand, CustomResult>,
        IRequestHandler<ResetSecurityCodeCommand, CustomResult>,
        IRequestHandler<VerifySecurityCodeCommand, CustomResult>
    {
        private const string HashKeySessionKey = "ph";
        private const string SecurityCodeTokenSessionKey = "SECURITYCODETOKEN";

        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;
        private readonly SecurityCodeService securityCodeService;
        private readonly ProfilePictureServices customerProfileService;
        private readonly IUserSession userSession;
        private readonly IUser user;
        private readonly IAesService aes;

        public SecurityCodeHandler(
          SecurityCodeService securityCodeService,
          ProfilePictureServices customerProfileService,
          SessionManager sessionManager,
          Encryptor encryptor,
          IUserSession userSession,
          IUser user,
          IAesService aes)
        {
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.securityCodeService = securityCodeService;
            this.customerProfileService = customerProfileService;
            this.userSession = userSession;
            this.user = user;
            this.aes = aes;
        }

        private string HashKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(sessionManager.Get<string>(HashKeySessionKey)))
                {
                    var hashKey = encryptor.RandomString(4);

                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return sessionManager.Get<string>(HashKeySessionKey);
            }
        }

        private string Decrypt(string encryptedData)
        {
            if (string.IsNullOrWhiteSpace(encryptedData))
            {
                return string.Empty;
            }

            return aes.Decrypt(encryptedData, HashKey);
        }

        public SecurityCodeViewModel Handle(SecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            string securityCodeToken = securityCodeService.GenerateSecurityToken();

            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return new SecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GenerateSecuritySalt(),
                HashKey = HashKey,
                CustRoleId = message.RoleId,
                Title = BuildSecurityCodeTitle(message.Username),
                UsernameShower = BuildUsernameShower(message.IsSubAcc, message.SubUserName, message.NickName, message.Username)
            };
        }

        public ForceChangeSecurityCodeViewModel Handle(ForceChangeSecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var securityCodeToken = securityCodeService.GenerateSecurityToken();

            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            var profile = customerProfileService.GetActorProfilePicture(this.userSession.CustId);

            return new ForceChangeSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GenerateSecuritySalt(),
                HashKey = HashKey,
                CustRoleId = message.RoleId,
                Username = message.UserName,
                AlternateUsername = BuildAlternateUsername(message),
                UsernameShower = BuildUsernameShower(message.IsSubAcc, message.SubUserName, message.NickName, message.UserName),
                IsDefaultAvatar = string.IsNullOrEmpty(profile.ImageBase64String),
                EmbedImage = profile.ImageBase64String
            };
        }

        public CustomResult Handle(ChangeSecurityCodeFirstLoginCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            // Make sure that, when user already has SC in session, then cannot be forced change by first login
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                return CustomResult.CreateErrorResult(Security.NotAuthorized);
            }

            message.SecurityCode = Decrypt(message.SecurityCode);
            var validateResult = HasUpdatePermission();

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var result = ChangeSecurityCode(new ChangeSecurityCodeParam
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                NewSecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = message.IP,
                MustVerifyCurrentSecurityCode = false
            });

            if (result.IsSuccessful)
            {
                if (string.IsNullOrWhiteSpace(userSession.SecurityCode))
                {
                    userSession.SecurityCode = message.SecurityCode;
                }

                ResetToken();
            }

            return result;
        }

        public CustomResult Handle(ChangeSecurityCodeByForcingCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            message.SecurityCode = Decrypt(message.SecurityCode);
            var validateResult = HasUpdatePermission();

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var result = ChangeSecurityCode(new ChangeSecurityCodeParam
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                NewSecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = message.IP,
                CurrentSecurityCode = message.OldSecurityCode,
                Salt = sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                MustVerifyCurrentSecurityCode = true
            });

            if (result.IsSuccessful)
            {
                userSession.SecurityCode = message.SecurityCode;
                ResetToken();
            }

            return result;
        }

        public CustomResult Handle(ChangeUserOwnSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            message.SecurityCode = Decrypt(message.SecurityCode);
            var validateResult = HasUpdatePermission();

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var param = new ChangeSecurityCodeParam
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                NewSecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = message.IP,
                CurrentSecurityCode = message.OldSecurityCode,
                Salt = sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                MustVerifyCurrentSecurityCode = true
            };

            var result = ChangeSecurityCode(param);

            if (result.IsSuccessful)
            {
                ResetToken();
            }

            return result;
        }

        public CustomResult Handle(ResetSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            message.SecurityCode = Decrypt(message.SecurityCode);
            bool isResetSubAccount = message.SubAccId.HasValue && message.SubAccId.Value > 0;

            var validateResult = HasUpdatePermission(isResetSubAccount);

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var result = new CustomResult();
            if (message.CustId.HasValue && message.CustId.Value > 0)
            {
                result = ResetDownlineSecurityCode(message);
            }
            else if (isResetSubAccount)
            {
                result = ResetSubAccountSecurityCode(message);
            }

            return result;
        }

        public CustomResult Handle(VerifySecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var result = securityCodeService.VerifySecurityCode(
                new VerifySecurityCodeParam
                {
                    CustId = message.CustId,
                    SecurityCode = message.SecurityCode,
                    IP = message.IP,
                    Language = userSession.Language,
                    Salt = sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                    SubAccId = message.SubAccId
                });

            if (result.IsSuccessful)
            {
                ResetToken();
            }

            return result;
        }

        private CustomResult HasUpdatePermission(bool isResetSubAccount = false)
        {
            if (userSession.IsInternal
                && !isResetSubAccount)
            {
                return new CustomResult { Code = CustomResult.ErrorCode, Message = Security.NotAuthorized };
            }

            if (isResetSubAccount && !user.IsAdminCanUpdateSubAccount)
            {
                return new CustomResult { Code = CustomResult.ErrorCode, Message = Security.NotAuthorized };
            }

            return new CustomResult();
        }

        private void ResetToken()
        {
            sessionManager.Add(SecurityCodeTokenSessionKey, string.Empty);
            userSession.IsForcedSecurityCode = false;
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        private CustomResult ChangeSecurityCode(ChangeSecurityCodeParam param)
        {
            var result = param.SubAccId.Equals(0)
                ? securityCodeService.ChangeMainAccountSecurityCode(param)
                : securityCodeService.ChangeSubAccountSecurityCode(param);

            if (result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private CustomResult ResetDownlineSecurityCode(ResetSecurityCodeCommand command)
        {
            var isResetMember = Enumeration.FromValue<UserLevel>(userSession.RoleId).Equals(UserLevel.Agent);

            var result = securityCodeService.ChangeMainAccountSecurityCode(
                new ChangeSecurityCodeParam
                {
                    CustId = command.CustId.Value,
                    UserId = userSession.UserId,
                    SubAccId = userSession.SubAccId,
                    IsResetMember = isResetMember,
                    NewSecurityCode = command.SecurityCode,
                    Language = command.Language,
                    IP = command.IP,
                    MustVerifyCurrentSecurityCode = false
                });

            if (result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private CustomResult ResetSubAccountSecurityCode(ResetSecurityCodeCommand command)
        {
            var result = securityCodeService.ResetSubAccountSecurityCode(
                new ResetSecurityCodeParam
                {
                    CustId = userSession.CustId,
                    UserId = userSession.UserId,
                    SubAccId = command.SubAccId.Value,
                    NewSecurityCode = command.SecurityCode,
                    Language = command.Language,
                    IP = command.IP
                });

            if (result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private string BuildUsernameShower(bool isSubAcc, string subUsername, string nickName, string username)
        {
            if (isSubAcc)
            {
                return subUsername;
            }

            return string.IsNullOrWhiteSpace(nickName) ? username : nickName;
        }

        private string BuildAlternateUsername(ForceChangeSecurityCodeQuery message)
        {
            if (message.IsSubAcc)
            {
                return $"({Resources.subAcc.ToUpper()} {message.SubUserName})";
            }

            return string.IsNullOrWhiteSpace(message.NickName) ? string.Empty : $"({message.NickName})";
        }

        private string GenerateSecuritySalt()
        {
            var actualCustId = userSession.SubAccId > 0 ? userSession.SubAccId : userSession.CustId;

            return securityCodeService.GenerateSalt(actualCustId);
        }

        private string BuildSecurityCodeTitle(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Security.titleResetSecCode;
            }

            return $"{Security.titleResetSecCode} - {username}";
        }
    }
}