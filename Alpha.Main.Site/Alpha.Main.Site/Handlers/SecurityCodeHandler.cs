namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using Alpha.SecurityCode.Services;
    using AppResources;
    using Main.Models;
    using Main.Models.CustomerProfiles;
    using Main.Models.Enumerations;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Params.SecurityCode;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using ViewModels.Security;

    public class SecurityCodeHandler : IRequestHandler<LoginSecurityCodeQuery, LoginSecurityCodeViewModel>,
         IRequestHandler<BaseSecurityCodeQuery, BaseSecurityCodeViewModel>,
         IRequestHandler<ChangeSecurityCodeFirstLoginCommand, Result>,
         IRequestHandler<ChangeSecurityCodeByForcingCommand, Result>,
         IRequestHandler<ChangeUserOwnSecurityCodeCommand, Result>,
         IRequestHandler<ResetSecurityCodeCommand, Result>,
         IRequestHandler<VerifySecurityCodeCommand, Result>,
         IRequestHandler<CheckLoginSecurityCodeQuery, SecurityCodeStatus>
    {
        private const string HashKeySessionKey = "ph";
        private const string SecurityCodeTokenSessionKey = "SECURITYCODETOKEN";
        private const string SecurityCodeResultSessionKey = "SECURITYCODERESULT";
        private readonly CustomerProfileService customerProfileService;
        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;
        private readonly SecurityCodeService securityCodeService;
        private readonly IUserSession userSession;
        private readonly SecurityCodeChecker securityCodeChecker;

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
            this.securityCodeChecker = securityCodeChecker;
        }

        private string HashKey
        {
            get
            {
                if (string.IsNullOrEmpty(sessionManager.Get<string>(HashKeySessionKey)))
                {
                    var hashKey = encryptor.RandomString(new Random().Next(4, 8));

                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return sessionManager.Get<string>(HashKeySessionKey);
            }
        }

        private string GetRC4Decryption(string encryption)
        {
            if (string.IsNullOrEmpty(encryption))
            {
                return string.Empty;
            }

            return encryptor.RC4(HashKey, encryptor.HexDecode(encryption));
        }

        public BaseSecurityCodeViewModel Handle(BaseSecurityCodeQuery message)
        {
            var profile = customerProfileService.GetProfilePicture(userSession.CustId);
            string securityCodeToken = securityCodeService.GenerateSecurityToken(6);
            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            var model = new BaseSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GetSecuritySalt(),
                HashKey = HashKey,
                CustRoleId = message.RoleId
            };

            return model;
        }

        public LoginSecurityCodeViewModel Handle(LoginSecurityCodeQuery message)
        {
            var profile = customerProfileService.GetProfilePicture(userSession.CustId);
            string securityCodeToken = securityCodeService.GenerateSecurityToken(6);
            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return new LoginSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GetSecuritySalt(),
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
            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            var validateResult = ValidateSecurityCode(message.SecurityCode);

            if (!validateResult.IsOK)
            {
                return validateResult;
            }

            Result result = UpdateSecurityCode(new SecurityCodeParams
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                SecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = message.IP
            });

            if (result.IsSuccessful)
            {
                userSession.SecurityCode = message.SecurityCode;
                ResetToken();
            }

            return result;
        }

        public Result Handle(ChangeSecurityCodeByForcingCommand message)
        {
            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            var validateResult = ValidateSecurityCode(message.SecurityCode, message.OldSecurityCode, true);

            if (!validateResult.IsOK)
            {
                return validateResult;
            }

            Result result = securityCodeService.CompareHashSecurityCodeBetweenWebAndDB(
               userSession.CustId,
               userSession.SubAccId,
               message.IsSubAcc,
               sessionManager.Get<string>(SecurityCodeTokenSessionKey),
               userSession.Language,
               message.OldSecurityCode,
               message.IP);

            if (result.IsOK)
            {
                result = UpdateSecurityCode(new SecurityCodeParams
                {
                    CustId = message.CustId,
                    SubAccId = message.SubAccId,
                    UserId = message.UserId,
                    SecurityCode = message.SecurityCode,
                    Language = message.Language,
                    IP = message.IP
                });
            }

            if (result.IsSuccessful)
            {
                userSession.SecurityCode = message.SecurityCode;
                ResetToken();
            }

            return result;
        }

        public Result Handle(ChangeUserOwnSecurityCodeCommand message)
        {
            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            var validateResult = ValidateSecurityCode(message.SecurityCode, message.OldSecurityCode, true);

            if (!validateResult.IsOK)
            {
                return validateResult;
            }

            Result result = securityCodeService.CompareHashSecurityCodeBetweenWebAndDB(
               userSession.CustId,
               userSession.SubAccId,
               message.IsSubAcc,
               sessionManager.Get<string>(SecurityCodeTokenSessionKey),
               userSession.Language,
               message.OldSecurityCode,
               message.IP);

            if (result.Code.Equals(0))
            {
                result = UpdateSecurityCode(new SecurityCodeParams
                {
                    CustId = message.CustId,
                    SubAccId = message.SubAccId,
                    UserId = message.UserId,
                    SecurityCode = message.SecurityCode,
                    Language = message.Language,
                    IP = message.IP
                });
            }

            if (result.IsSuccessful)
            {
                ResetToken();
            }

            return result;
        }

        public Result Handle(ResetSecurityCodeCommand message)
        {
            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            bool isResetSubAccount = message.SubAccId.HasValue && message.SubAccId.Value > 0;

            var validateResult = ValidateSecurityCode(message.SecurityCode, null, false, isResetSubAccount);

            if (!validateResult.IsOK)
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
            if (userSession.IsInternal)
            {
                return new Result { Code = Result.ErrorCode, Message = Security.NotAuthorized };
            }

            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                return new Result { Code = Result.ErrorCode, Message = Security.informSecCode };
            }

            Result result = securityCodeService.CompareHashSecurityCodeBetweenWebAndDB(
                message.CustId,
                message.SubAccId,
                message.IsSubAcc,
                sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                userSession.Language,
                message.SecurityCode,
               message.IP);

            if (result.IsSuccessful)
            {
                ResetToken();
            }

            return result;
        }

        private Result ValidateSecurityCode(
            string securityCode,
            string oldSecurityCode = null,
            bool isCheckOldSecurityCode = false,
            bool isResetSubAccount = false)
        {
            if (userSession.IsInternal && !isResetSubAccount)
            {
                return new Result { Code = Result.ErrorCode, Message = Security.NotAuthorized };
            }

            string errorMessage = string.Empty;

            if ((isCheckOldSecurityCode && string.IsNullOrWhiteSpace(oldSecurityCode))
                 || string.IsNullOrWhiteSpace(securityCode))
            {
                errorMessage = Security.alertOldSecCodeEmpty;
            }

            if (!securityCodeService.ValidateSecurityCode(securityCode))
            {
                errorMessage = Security.lblSecCodeWrongRule;
            }

            if (errorMessage.Length != 0)
            {
                return new Result { Code = Result.ErrorCode, Message = errorMessage };
            }

            return new Result();
        }

        private void ResetToken()
        {
            sessionManager.Add(SecurityCodeTokenSessionKey, string.Empty);
            sessionManager.Add(SecurityCodeResultSessionKey, GetSecurityCodeResult());
            userSession.IsForcedSecurityCode = false;
            Alpha.SecurityCode.Web.SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        private Result UpdateSecurityCode(SecurityCodeParams securityCodeParams)
        {
            Result result = securityCodeParams.SubAccId.Equals(0) ?
                securityCodeService.UpdateMainAccountHashSecurityCode(securityCodeParams) : securityCodeService.UpdateSubAccountHashSecurityCode(securityCodeParams);

            if (result != null && result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private Result ResetDownlineSecurityCode(ResetSecurityCodeCommand command)
        {
            var isResetMember = Enumeration.FromValue<UserLevel>(userSession.RoleId).Equals(UserLevel.Agent);

            Result result = securityCodeService.UpdateMainAccountHashSecurityCode(new SecurityCodeParams
            {
                CustId = command.CustId.Value,
                UserId = userSession.UserId,
                SubAccId = userSession.SubAccId,
                IsResetMember = isResetMember,
                SecurityCode = command.SecurityCode,
                Language = command.Language,
                IP = command.IP
            });

            if (result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private Result ResetSubAccountSecurityCode(ResetSecurityCodeCommand command)
        {
            Result result = securityCodeService.ResetSubAccountHashSecurityCode(new SecurityCodeParams
            {
                CustId = userSession.CustId,
                UserId = userSession.UserId,
                SubAccId = command.SubAccId.Value,
                SecurityCode = command.SecurityCode,
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

        private string BuildUsernameShower(LoginSecurityCodeQuery message)
        {
            if (message.IsSubAcc)
            {
                return message.SubUserName;
            }
            else
            {
                return string.IsNullOrEmpty(message.NickName) ? message.UserName : message.NickName;
            }
        }

        private string BuildAlternateUsername(LoginSecurityCodeQuery message)
        {
            if (message.IsSubAcc)
            {
                return $"({Resources.subAcc.ToUpper()} {message.SubUserName})";
            }
            else
            {
                return string.IsNullOrEmpty(message.NickName) ? string.Empty : $"({message.NickName})";
            }
        }

        private string GetSecuritySalt()
        {
            var securitySalt = userSession.SubAccId > 0
               ? securityCodeService.MD5Salt(userSession.SubAccId)
               : securityCodeService.MD5Salt(userSession.CustId);

            return securitySalt;
        }

        private SecurityCodeStatus GetSecurityCodeResult()
        {
            if (!securityCodeChecker.IsEnableSecurityCode())
            {
                return (int)SecurityCodeStatus.None;
            }

            object value = sessionManager.Get<string>(SecurityCodeResultSessionKey);
            if (value != null)
            {
                return (SecurityCodeStatus)value;
            }

            SecurityCodeStatus status = SecurityCodeStatus.ProvideSecurityCode;
            bool isForceSecurityCode = userSession.IsForcedSecurityCode;
            bool hasSC = !string.IsNullOrEmpty(userSession.SecurityCode);
            if (isForceSecurityCode)
            {
                if (userSession.IsInternal)
                {
                    status = SecurityCodeStatus.ProvideSecurityCodeSuccess;
                }
                else
                {
                    status = hasSC
                        ? SecurityCodeStatus.ChangeSecurityCodeByForcing
                        : SecurityCodeStatus.ChangeSecurityCodeFirstLogin;
                }
            }
            else
            {
                if (hasSC)
                {
                    status = userSession.IsInternal ? SecurityCodeStatus.ProvideSecurityCodeSuccess : SecurityCodeStatus.ProvideSecurityCode;
                }
                else
                {
                    status = SecurityCodeStatus.None;
                }
            }

            if (userSession.CustId > 0)
            {
                sessionManager.Add(SecurityCodeResultSessionKey, status);
            }

            sessionManager.Add(SecurityCodeResultSessionKey, status);

            return status;
        }

        public SecurityCodeStatus Handle(CheckLoginSecurityCodeQuery message)
        {
            return GetSecurityCodeResult();
        }
    }
}