namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Alpha.Main.Services;
    using AppResources;
    using Criteria.SecurityCode;
    using Fanex.Data.Repository;
    using Models;
    using Params.SecurityCode;
    using Sunplus.Agent.Utilities;

    public class SecurityCodeServiceImpl : SecurityCodeService
    {
        private const string MD5 = "MD5";
        private const string SHA1 = "SHA1";
        private const string SHA256 = "SHA256";
        private const string SECURITYCODEPADDING = "1qaw3+{:P)";
        private const int AllowSecurityCodeLength = 6;
        private const string SecurityCodeRule = @"^([0-9])(?!\1+$)[0-9]+$";
        private const string Message = "Param cannot be null";
        private const int Length = 6;
        private const int AccountClosed = 200;
        private const int AccountClosedStatus = 2;
        private const int WrongSecurityCode = 9009;
        private readonly IDynamicRepository dynamicRepository;
        private readonly Encryptor encryptor;
        private readonly INetThrottlingService netThrottlingService;
        private readonly TelegramBotService telegramBotService;

        public SecurityCodeServiceImpl(
            IDynamicRepository dynamicRepository,
            Encryptor encryptor,
            INetThrottlingService netThrottlingService,
            TelegramBotService telegramBotService)
        {
            this.dynamicRepository = dynamicRepository;
            this.encryptor = encryptor;
            this.netThrottlingService = netThrottlingService;
            this.telegramBotService = telegramBotService;
        }

        public Result ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), Message);
            }

            var validateResult = Validate(param.NewSecurityCode, param);
            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var command = new UpdateMainAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                UserId = param.UserId,
                HashedSecurityCode = HashSecurityCode(param.CustId, param.NewSecurityCode, param.IsResetMember),
                IP = param.IP,
                Language = param.Language
            };
            dynamicRepository.Execute(command);
            var result = new Result { Code = command.ErrCode, Message = command.ErrMsg };

            if (result.IsSuccessful && !param.IsMemberLevel)
            {
                Task.Run(() => telegramBotService.NotifyChangeSecurityCode(param.CustId, 0));
            }

            return result;
        }

        public Result ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), Message);
            }

            var validateResult = Validate(param.NewSecurityCode, param);
            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var command = new UpdateSubAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                HashedSecurityCode = HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                IP = param.IP,
                Language = param.Language
            };

            dynamicRepository.Execute(command);
            var result = new Result { Code = command.ErrCode, Message = command.ErrMsg };

            if (result.IsSuccessful && !param.IsMemberLevel)
            {
                Task.Run(() => telegramBotService.NotifyChangeSecurityCode(param.CustId, param.SubAccId));
            }

            return result;
        }

        public Result ResetSubAccountSecurityCode(ResetSecurityCodeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), Message);
            }

            if (netThrottlingService.IsBlacklist())
            {
                return Result.CreateErrorResult("Too many requests");
            }

            var validateResult = Validate(param.NewSecurityCode);
            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var command = new ResetSubAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                UserId = param.UserId,
                HashedSecurityCode = HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                IP = param.IP,
                Language = param.Language
            };

            dynamicRepository.Execute(command);
            var result = new Result { Code = command.ErrCode, Message = command.ErrMsg };

            if (result.IsSuccessful && !param.IsMemberLevel)
            {
                netThrottlingService.Throttle();

                Task.Run(() => telegramBotService.NotifyChangeSecurityCode(param.CustId, param.SubAccId));
            }

            return result;
        }

        private string HashSecurityCode(int custId, string securityCode, bool isResetMember = false)
        {
            //// because the Member site doesn't want to edit his code.
            //// so that we must use the CfsEnCode method.
            if (isResetMember)
            {
                return encryptor.CfsEnCode(securityCode);
            }

            var salt = GenerateSalt(custId);

            return encryptor.ComputeHash(SHA1, securityCode, salt);
        }

        public string GenerateSalt(int custId)
        {
            string plainText = custId.ToString();

            return encryptor.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
        }

        private Result Validate(
            string securityCode,
            ChangeSecurityCodeParam param = null)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                return Result.CreateErrorResult(Security.alertSecCodeEmpty);
            }

            if (!IsMatchedRules(securityCode))
            {
                return Result.CreateErrorResult(Security.lblSecCodeWrongRule);
            }

            if (param != null && param.MustVerifyCurrentSecurityCode)
            {
                if (string.IsNullOrWhiteSpace(param.CurrentSecurityCode))
                {
                    return Result.CreateErrorResult(Security.alertOldSecCodeEmpty);
                }

                var verifyResult = VerifySecurityCode(new VerifySecurityCodeParam
                {
                    CustId = param.CustId,
                    SecurityCode = param.CurrentSecurityCode,
                    IP = param.IP,
                    Language = param.Language,
                    Salt = param.Salt,
                    SubAccId = param.SubAccId
                });

                if (verifyResult.IsNotOk)
                {
                    return verifyResult;
                }
            }

            return new Result();
        }

        private bool IsMatchedRules(string securityCode)
        {
            int securityCodeLength = securityCode == null ? 0 : securityCode.Trim().Length;
            if (securityCodeLength != AllowSecurityCodeLength)
            {
                return false;
            }

            Regex regex = new Regex(SecurityCodeRule);
            //// check digits and all of them are not the same
            bool isMatched = regex.IsMatch(securityCode);
            if (isMatched)
            {
                //// check 'Forward Run' and 'Backward Run'
                bool isForwardRun = true, isBackwardRun = true;
                int forward = 0, backward = 0;
                forward = backward = int.Parse(securityCode[0].ToString());
                for (int i = 1; i < securityCodeLength; i++)
                {
                    int current = int.Parse(securityCode[i].ToString());
                    if (++forward != current)
                    {
                        isForwardRun = false;
                    }

                    if (--backward != current)
                    {
                        isBackwardRun = false;
                    }
                }

                isMatched = !(isForwardRun || isBackwardRun);
            }

            return isMatched;
        }

        public Result VerifySecurityCode(VerifySecurityCodeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), Message);
            }

            if (string.IsNullOrEmpty(param.SecurityCode))
            {
                return Result.CreateErrorResult(Security.informSecCode);
            }

            if (string.IsNullOrEmpty(param.SecurityCode)
                || string.IsNullOrEmpty(param.Salt))
            {
                return Result.CreateErrorResult(Security.alertInputWrongSecCode);
            }

            // Database store Security Code hashed with CustID/SubAccID Salt
            // But Security code send from client browser has been hashed with CustID/SubAccID Salt by SHA256 Algorithm
            // So we have to hased security code from database with CustID/SubAccID Salt by SHA256 first, then compare
            // with hased security code which was sent from client browser.
            var actualSecurityCode = GetSecurityCode(param.CustId, param.SubAccId);
            var hashedActualSecurityCodeWithSalt = this.encryptor.ComputeHash(SHA256, actualSecurityCode, param.Salt);
            var isCorrectSecurityCode = string.Equals(param.SecurityCode, hashedActualSecurityCodeWithSalt);

            //// update counter after verify
            return UpdateSecurityCodeCounter(param.CustId, isCorrectSecurityCode, param.Language, param.SubAccId, param.IP);
        }

        public string GenerateSecurityToken()
        {
            return this.encryptor.RandomString(Length);
        }

        private string GetSecurityCode(int custId, int subAccountId)
        {
            CriteriaBase criteria;

            if (subAccountId.Equals(0))
            {
                criteria = new GetMainAccountSecurityCodeCriteria { CustId = custId };
            }
            else
            {
                criteria = new GetSubAccountSecurityCodeCriteria { CustId = custId, SubaccId = subAccountId };
            }

            return dynamicRepository.Get<string>(criteria);
        }

        private Result UpdateSecurityCodeCounter(
            int custId,
            bool isCorrectSecurityCode,
            string language,
            int subAccountId,
            string ip)
        {
            UpdateSecurityCodeCriteria command;

            if (subAccountId.Equals(0))
            {
                command = new UpdateMainAccountSecurityCodeCounterCriteria
                {
                    CustId = custId,
                    CheckedSecurityCode = isCorrectSecurityCode,
                    IP = ip,
                    Language = language
                };
            }
            else
            {
                command = new UpdateSubAccountSecurityCodeCounterCriteria
                {
                    CustId = custId,
                    SubaccId = subAccountId,
                    CheckedSecurityCode = isCorrectSecurityCode,
                    IP = ip,
                    Language = language
                };
            }

            dynamicRepository.Execute(command);
            var result = new Result { Code = command.ErrCode, Message = command.ErrMsg };

            if (result.Code == AccountClosed)
            {
                result.Code = AccountClosedStatus; //// account is closed
            }
            else if (result.IsFailed)
            {
                result.Code = WrongSecurityCode; //// wrong SC
                result.Message = Security.alertInputWrongSecCode;
            }

            return result;
        }

        public Result UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param)
        {
            var command = new UpdateSecurityCodeFunctionCriteria(
                param.IsOn, param.Function, param.CustId, param.SubAccId, param.LangKey, param.IP);
            dynamicRepository.Execute(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg };
        }
    }
}
