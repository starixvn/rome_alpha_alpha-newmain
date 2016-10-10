namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using Alpha.SecurityCode.Services;
    using Alpha.SecurityCode.Web;
    using AppResources;
    using Main.Models;
    using Main.Models.CustomerProfiles;
    using Main.Models.Enumerations;
    using MediatR;
    using Models.Request.SecurityCode;
    using Params.SecurityCode;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using ViewModels.Security;

    public class SecurityCodeHandler :
        IRequestHandler<ForceChangeSecurityCodeQuery, ForceChangeSecurityCodeViewModel>,
        IRequestHandler<SecurityCodeQuery, SecurityCodeViewModel>,
        IRequestHandler<ChangeSecurityCodeFirstLoginCommand, Result>,
        IRequestHandler<ChangeSecurityCodeByForcingCommand, Result>,
        IRequestHandler<ChangeUserOwnSecurityCodeCommand, Result>,
        IRequestHandler<ResetSecurityCodeCommand, Result>,
        IRequestHandler<VerifySecurityCodeCommand, Result>
    {
        private const string HashKeySessionKey = "ph";
        private const string SecurityCodeTokenSessionKey = "SECURITYCODETOKEN";

        private readonly CustomerProfileService customerProfileService;
        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;
        private readonly SecurityCodeService securityCodeService;
        private readonly IUserSession userSession;

        public SecurityCodeHandler(
          SecurityCodeService securityCodeService,
          CustomerProfileService customerProfileService,
          SessionManager sessionManager,
          Encryptor encryptor,
          IUserSession userSession,
          SecurityCodeChecker securityCodeChecker)
        {
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.customerProfileService = customerProfileService;
            this.securityCodeService = securityCodeService;
            this.userSession = userSession;
        }

        private string HashKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(sessionManager.Get<string>(HashKeySessionKey)))
                {
                    var hashKey = encryptor.RandomString(new Random().Next(4, 8));

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

            return encryptor.RC4(HashKey, encryptor.HexDecode(encryptedData));
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
                CustRoleId = message.RoleId
            };
        }

        public ForceChangeSecurityCodeViewModel Handle(ForceChangeSecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var profile = customerProfileService.GetProfilePicture(userSession.CustId);
            var securityCodeToken = securityCodeService.GenerateSecurityToken();

            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return new ForceChangeSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GenerateSecuritySalt(),
                HashKey = HashKey,
                CustRoleId = message.RoleId,
                IsDefaultAvatar = profile.IsDefaultAvatar,
                Username = message.UserName,
                AlternateUsername = BuildAlternateUsername(message),
                UsernameShower = BuildUsernameShower(message),
                EmbedImage = GenerateImage(profile)
            };
        }

        public Result Handle(ChangeSecurityCodeFirstLoginCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            // Make sure that, when user already has SC in session, then cannot be forced change by first login
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
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

        public Result Handle(ChangeSecurityCodeByForcingCommand message)
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

        public Result Handle(ChangeUserOwnSecurityCodeCommand message)
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
                ResetToken();
            }

            return result;
        }

        public Result Handle(ResetSecurityCodeCommand message)
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

            Result result = new Result();
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

        public Result Handle(VerifySecurityCodeCommand message)
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
                    SubAccId = message.SubAccId,
                    IsSubAcc = message.IsSubAcc
                });

            if (result.IsSuccessful)
            {
                ResetToken();
            }

            return result;
        }

        private Result HasUpdatePermission(bool isResetSubAccount = false)
        {
            if (userSession.IsInternal
                && !isResetSubAccount)
            {
                return new Result { Code = Result.ErrorCode, Message = Security.NotAuthorized };
            }

            return new Result();
        }

        private void ResetToken()
        {
            sessionManager.Add(SecurityCodeTokenSessionKey, string.Empty);
            userSession.IsForcedSecurityCode = false;
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        private Result ChangeSecurityCode(ChangeSecurityCodeParam param)
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

        private Result ResetDownlineSecurityCode(ResetSecurityCodeCommand command)
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

        private Result ResetSubAccountSecurityCode(ResetSecurityCodeCommand command)
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

        private string GenerateImage(ProfilePicture profilePicture)
        {
            if (profilePicture.BinaryAvatar != null)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.BinaryAvatar)}";
            }

            return string.Empty;
        }

        private string BuildUsernameShower(ForceChangeSecurityCodeQuery message)
        {
            if (message.IsSubAcc)
            {
                return message.SubUserName;
            }

            return string.IsNullOrWhiteSpace(message.NickName) ? message.UserName : message.NickName;
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
    }
}