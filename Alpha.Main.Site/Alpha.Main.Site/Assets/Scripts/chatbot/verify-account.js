/// <reference path="../core.ts" />
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var VerifyAccount = /** @class */ (function (_super) {
    __extends(VerifyAccount, _super);
    function VerifyAccount() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.resources = {
            verificationCodeIsInvalidOrExpired: document.getElementById('VerificationCodeIsInvalidOrExpired').value,
            verificationCodeIsRequired: document.getElementById('VerificationCodeIsRequired').value,
            captchaIsRequired: document.getElementById('CaptchaIsRequired').value,
            accountHasAlreadyBeenRegistered: document.getElementById('AccountHasAlreadyBeenRegistered').value
        };
        _this.init = function () {
            _this.isShowCaptcha = _this.convertAttributeToBool(_this.$("#IsShowCaptcha").val());
            _this.cachedDOMElements();
            _this.registerEvents();
            if (_this.isShowCaptcha) {
                _this.cachedCaptchaDOMElements();
                _this.registerCaptchaEvents();
                _this.$captcha.show();
            }
            var expiredTime = Number(localStorage.getItem(_this.expiredTimeKey));
            var currentTime = new Date().getTime();
            if (expiredTime && currentTime < expiredTime) {
                var sec = (expiredTime - currentTime) / 1000;
                _this.$countDown.text(Math.floor(sec));
                _this.countDownTimer();
            }
            else {
                _this.$countDown.text(0);
                _this.$btnVerify.attr("disabled", "disabled");
            }
        };
        _this.cachedDOMElements = function () {
            _this.$verificationSection = _this.$("#verification-section");
            _this.$verificationForm = _this.$("#verification-form");
            _this.$txtVerificationCode = _this.$("#txtVerificationCode");
            _this.$verificationMessage = _this.$("#verification-code-message");
            _this.$captchaContainer = _this.$("#captcha-container");
            _this.$btnVerify = _this.$("#btnVerify");
            _this.$btnResend = _this.$("#btnResend");
            _this.$countDown = _this.$("#countdown");
        };
        _this.registerEvents = function () {
            _this.$btnVerify.click(_this.verifyCodeEvent);
            _this.$btnResend.click(_this.resendEvent);
            _this.$txtVerificationCode.keypress(function (event) {
                if (event.keyCode === 13 && !_this.$btnVerify.is(":disabled")) {
                    _this.$btnVerify.click();
                }
            });
        };
        _this.initCaptcha = function () {
            _this.cachedCaptchaDOMElements();
            _this.registerCaptchaEvents();
            _this.$captcha.show();
        };
        _this.cachedCaptchaDOMElements = function () {
            var captchaId = _this.$("#CaptchaId").val();
            _this.$captcha = _this.$("#" + captchaId);
            _this.$captchaRefreshIcon = _this.$("#captcha-refresh-icon");
            _this.$txtCaptcha = _this.$("#txtCaptcha");
            _this.imageElementId = _this.$("#ModelImageElementId").val();
            _this.$modelImageElement = _this.$("#" + _this.imageElementId);
            _this.$modelTokenParameterName = _this.$("#ModelTokenParameterName");
            _this.tokenElementId = _this.$("#ModelTokenElementId").val();
            _this.$modelTokenElement = _this.$("#" + _this.tokenElementId);
            _this.$modelRefreshUrl = _this.$("#ModelRefreshUrl");
        };
        _this.registerCaptchaEvents = function () {
            _this.$captchaRefreshIcon.click(_this.refreshCaptchaEvent);
            _this.$modelImageElement.click(_this.refreshCaptchaEvent);
            _this.$txtCaptcha.keypress(function (event) {
                if (event.keyCode === 13 && !_this.$btnVerify.is(":disabled")) {
                    _this.$btnVerify.click();
                }
            });
        };
        _this.verifyCodeEvent = function (e) {
            e.preventDefault();
            var verificationCode = _this.$txtVerificationCode.val();
            if (verificationCode.length === 0) {
                _this.$txtVerificationCode.focus();
                alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.verificationCodeIsRequired }));
                return;
            }
            else {
                _this.$verificationMessage.html("");
            }
            if (_this.isShowCaptcha) {
                if (_this.$txtCaptcha.val() === "") {
                    _this.$txtCaptcha.focus();
                    alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.captchaIsRequired }));
                    return;
                }
                else {
                    _this.$verificationMessage.html("");
                }
            }
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/VerifyVerificationCode",
                data: _this.$verificationForm.serialize(),
                dataType: "text",
                success: _this.onVerifySuccess,
                complete: _this.onComplete,
            });
        };
        _this.onVerifySuccess = function (data) {
            var result = _this.safelyParseJSON(data);
            if (result) {
                if (result.IsOk) {
                    parent.window.location.href = parent.window.location.href;
                }
                else {
                    if (result.IsNotAuthorized) {
                        _this.topWindow.bootboxAlert(result.Message);
                        return;
                    }
                    _this.isShowCaptcha = _this.convertAttributeToBool(_this.$("#IsShowCaptcha").val());
                    if (_this.isShowCaptcha) {
                        _this.reloadCaptcha();
                    }
                    if (result.IsVerificationCodeExpired || result.IsWrongVerificationCode) {
                        alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.verificationCodeIsInvalidOrExpired }));
                    }
                    else if (result.IsMessengerAccountHasBeenRegisteredByAnotherCustomer) {
                        alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.accountHasAlreadyBeenRegistered }));
                    }
                    else {
                        alpha.showMessage(_this.$verificationMessage, new Result({ Code: result.Code, Message: result.Message }));
                    }
                }
                return;
            }
            _this.$captchaContainer.html(data);
            _this.initCaptcha();
            alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.verificationCodeIsInvalidOrExpired }));
        };
        _this.resendEvent = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/ResendVerificationCode",
                dataType: "json",
                success: _this.onResendSuccess,
                complete: _this.onComplete,
            });
        };
        _this.onResendSuccess = function (data) {
            if (data) {
                if (data.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(data.Message);
                    return;
                }
                if (data.IsOk) {
                    clearInterval(_this.timer);
                    _this.updateExpiredTime();
                    _this.$countDown.text(179);
                    _this.$btnVerify.removeAttr("disabled");
                    _this.countDownTimer();
                    _this.$btnResend.attr("disabled", "disabled");
                    var interval = setInterval(function () {
                        clearInterval(interval);
                        _this.$btnResend.removeAttr("disabled");
                    }, 2000);
                }
            }
        };
        _this.refreshCaptchaEvent = function (e) {
            var self = _this;
            var tokenParams = {};
            tokenParams[_this.$modelTokenParameterName.val()] = _this.$modelTokenElement.val();
            var url = _this.$modelRefreshUrl.val();
            _this.$.post(url, tokenParams)
                .always(function (result) {
                eval(result.responseText);
                self.$captcha.show();
            });
            return false;
        };
        _this.reloadCaptcha = function () {
            _this.$.blockUI();
            _this.$.ajax({
                type: "GET",
                url: alpha.urls.mainRootPath + "ChatBot/Captcha",
                dataType: "text",
                success: function (result) {
                    _this.$captchaContainer.html(result);
                    _this.initCaptcha();
                },
                complete: _this.onComplete,
            });
        };
        _this.countDownTimer = function () {
            var sec = parseInt(_this.$countDown.text());
            _this.timer = setInterval(function () {
                if (sec == 0) {
                    _this.$btnVerify.attr("disabled", "disabled");
                    clearInterval(_this.timer);
                }
                else {
                    _this.$countDown.text(--sec);
                }
            }, 1000);
        };
        return _this;
    }
    return VerifyAccount;
}(BaseChatBot));
//# sourceMappingURL=verify-account.js.map