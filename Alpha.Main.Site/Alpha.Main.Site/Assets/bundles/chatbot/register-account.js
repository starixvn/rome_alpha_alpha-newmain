var alpha = {
    urls: {
        oldMainRootPath: top.document.getElementById('oldMainRootPath') != null
            ? top.document.getElementById('oldMainRootPath').value
            : "not found",
        mainRootPath: top.document.getElementById('mainRootPath') != null
            ? top.document.getElementById('mainRootPath').value
            : "not found",
        accountInfoRootPath: top.document.getElementById('accountInfoRootPath') != null
            ? top.document.getElementById('accountInfoRootPath').value
            : "not found",
        memberInfoRootPath: top.document.getElementById('memberInfoRootPath') != null
            ? top.document.getElementById('memberInfoRootPath').value
            : "not found",
        howToUseRootPath: top.document.getElementById('howToUseRootPath') != null
            ? top.document.getElementById('howToUseRootPath').value
            : "not found",
        exTotalBetsForecastRootPath: top.document.getElementById('exTotalBetsForecastRootPath') != null
            ? top.document.getElementById('exTotalBetsForecastRootPath').value
            : "not found",
        messageRootPath: top.document.getElementById('messageRootPath') != null
            ? top.document.getElementById('messageRootPath').value
            : "not found"
    },
    user: {
        onUserId: document.getElementById('onUserId') != null
            ? document.getElementById('onUserId').value
            : "not found",
        passwordHash: document.getElementById('passwordHash') != null
            ? document.getElementById('passwordHash').value
            : "not found",
        subPageAccess: document.getElementById('subPageAccess') != null
            ? document.getElementById('subPageAccess').value
            : "not found",
        level: document.getElementById('userLevel') != null
            ? document.getElementById('userLevel').value
            : "not found",
        isSubAccount: document.getElementById('isSubAccount') != null
            ? (document.getElementById('isSubAccount').value.toLowerCase() === "true")
            : "not found",
        isShowIntroduction: document.getElementById('isShowIntroduction') != null
            ? (document.getElementById('isShowIntroduction').value.toLowerCase() === "true")
            : "not found",
        isEnableOtp: document.getElementById('isEnabledOtp') != null
            ? (document.getElementById('isEnabledOtp').value.toLowerCase() === "true")
            : false
    },
    core: {
        convertReponseToJson: function (reponse) {
            return eval("(" + reponse + ")");
        },
        signOut: function () {
            var index = window;
            while (index != index.parent) {
                index = index.parent;
            }
            var isInternal = index.$("#isInternal").val();
            var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';
            if (isInternal.toString() !== "0") {
                $.ajax(signOutUrl, {
                    success: function () {
                        index.close();
                    }
                });
            }
            else {
                index.location = signOutUrl;
            }
        },
        isIe: function () {
            return /(MSIE|Trident\/|Edge\/)/i.test(navigator.userAgent);
        },
        IsIe8OrLower: function () {
            var userAgent = window.navigator.userAgent.toLowerCase();
            return (userAgent.indexOf('msie') != -1)
                ? (parseInt(userAgent.split('msie')[1]) < 9)
                : false;
        },
        IsAndroid: function () {
            return /Android/i.test(navigator.userAgent);
        }
    },
    isScreenGreater1024: false,
    isScreenLower768: false,
    isScreenGreater768AndLower1024: false,
    isScreenLower1024: false,
    isMobile: false,
    detectResponsiveInformation: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }
        var currentWindowWidth = window.screen.width;
        if (currentWindowWidth < 768) {
            alpha.isScreenLower768 = true;
        }
        else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        }
        else {
            alpha.isScreenGreater1024 = true;
        }
        alpha.isMobile = isMobile.any;
    },
    localStorage: {
        get: function (key) {
            return store.enabled ? store.get(key) : $.cookie(key);
        },
        set: function (key, value) {
            store.enabled ? store.set(key, value) : $.cookie(key, value, alpha.localStorage.getCookieExpireOptions());
        },
        check: function (key) {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },
        getCookieExpireOptions: function () {
            var cookieExpire = { expires: 365, path: window.location.pathname };
            if (alpha.core.isIe()) {
                cookieExpire = { expires: 365 };
            }
            return cookieExpire;
        },
    },
    showMessage: function ($target, result) {
        $target.empty();
        if (result.isSuccessful) {
            $target.html("<div class='msg msg-success'>" + result.message + "</div>");
            return;
        }
        if (result.isWarning) {
            $target.html("<div class='msg msg-warning'>" + result.message + "</div>");
            return;
        }
        if (result.isFailed) {
            $target.html("<div class='msg msg-error'>" + result.message + "</div>");
            return;
        }
    },
    key: {
        instruction: 'Instruction',
        mobileInstruction: 'MobileInstruction'
    },
    messageType: {
        Normal: 0,
        Personal: 1,
        Special: 2,
        System: 3
    }
};
$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken[0].value);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 401) {
                alert('Oops... you are here because of a system error, please notify the administrators.');
                alpha.core.signOut();
            }
        }
    });
});
var Result = /** @class */ (function () {
    function Result(data) {
        this.code = data.Code;
        this.message = data.Message || "";
        this.isFailed = this.code !== 0;
        this.isSuccessful = this.code === 0;
        this.isWarning = data.Code === Result.warningCode;
    }
    Result.warningCode = -1000;
    return Result;
}());
//# sourceMappingURL=core.js.map
var BaseChatBot = /** @class */ (function () {
    function BaseChatBot($, topWindow) {
        var _this = this;
        this.expiredTimeKey = "ChatbotVerificationExpiredTime";
        this.baseResources = {
            reportExample: document.getElementById('ReportExample').value,
            notificationExample: document.getElementById('NotificationExample').value,
            actionExample: document.getElementById('ActionExample').value
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
        this.safelyParseJSON = function (json) {
            var parsed;
            try {
                parsed = JSON.parse(json);
            }
            catch (e) { }
            return parsed;
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.updateExpiredTime = function () {
            var currentTime = new Date();
            var expiredTime = currentTime.getTime() + (3 * 60000); //3 minutes
            localStorage.setItem(_this.expiredTimeKey, expiredTime.toString());
        };
        this.cacheDOMAndRegisterFunctionExample = function (getLanguageFunc) {
            _this.$openReportExample = _this.$("#openReportExample");
            _this.$openReportExample.on('click touchend', function () {
                _this.openExample(_this.baseResources.reportExample, "ChatBot/ReportExample", 400, getLanguageFunc());
            });
            _this.$openNotificationExample = _this.$("#openNotificationExample");
            _this.$openNotificationExample.on('click touchend', function () {
                _this.openExample(_this.baseResources.notificationExample, "ChatBot/NotificationExample", 336, getLanguageFunc());
            });
            _this.$openActionExample = _this.$("#openActionExample");
            _this.$openActionExample.on('click touchend', function () {
                _this.openExample(_this.baseResources.actionExample, "ChatBot/ActionExample", 400, getLanguageFunc());
            });
        };
        this.openExample = function (title, url, height, language) {
            var src = alpha.urls.mainRootPath + url + "?language=" + language;
            _this.topWindow.bootbox.dialog({
                title: title,
                message: '<iframe height="' + height + 'px" width="100%" src="' + src + '"></iframe>'
            });
            var dialog = _this.topWindow.$(".bootbox .modal-dialog");
            dialog.width(400);
            dialog.css({
                left: _this.topWindow.$(_this.topWindow.window).width() / 2 - dialog.width() / 2,
                top: 100
            });
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    return BaseChatBot;
}());
//# sourceMappingURL=base-chatbot.js.map
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
/// <reference path="../core.ts" />
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
var AddAccount = /** @class */ (function (_super) {
    __extends(AddAccount, _super);
    function AddAccount($, topWindow) {
        var _this = _super.call(this, $, topWindow) || this;
        _this.resources = {
            accountDoesNotExist: document.getElementById('AccountDoesNotExist').value,
            unsubscribeWarning: document.getElementById('UnsubscribeWarning').value,
            overMaxAddAccountTimes: document.getElementById('OverMaxAddAccountTimes').value,
            accountHasAlreadyBeenRegistered: document.getElementById('AccountHasAlreadyBeenRegistered').value,
            accountIsRequired: document.getElementById('AccountIsRequired').value,
            language: document.getElementById('Language').value
        };
        _this.init = function () {
            _this.cachedDOMElements();
            _this.registerEvents();
            var isVerifying = _this.convertAttributeToBool(_this.$("#IsVerifying").val());
            if (isVerifying) {
                _this.verifyAccount.init();
            }
            _this.cacheDOMAndRegisterFunctionExample(function () { return _this.resources.language; });
        };
        _this.cachedDOMElements = function () {
            _this.$registerLetstalkPanel = _this.$("#registerLetstalkPanel");
            _this.$txtAccount = _this.$("#txtAccount");
            _this.$addAccountMessage = _this.$("#add-account-message");
            _this.$btnAdd = _this.$("#btnAdd");
            _this.$btnUnsubscribe = _this.$("#btnUnsubscribe");
            _this.$logoLetstalk = _this.$("#logoLetstalk");
            _this.$imgQRCode = _this.$("#imgQRCode");
            _this.$linkShowQRCode = _this.$("#showQRCode");
            _this.$linkHideQRCode = _this.$("#hideQRCode");
            _this.$divLinkTooltip = _this.$("#divLinkTooltip");
        };
        _this.registerEvents = function () {
            _this.$txtAccount.keypress(function (e) {
                if (e.which === 32) {
                    return false;
                }
                if (e.keyCode === 13) {
                    _this.$btnAdd.click();
                }
            });
            _this.$btnAdd.click(_this.addAccountEvent);
            _this.$btnUnsubscribe.click(_this.unsubscribeEvent);
            _this.$linkShowQRCode.click(function () {
                _this.$logoLetstalk.addClass("hide");
                _this.$imgQRCode.removeClass("hide");
                _this.$linkShowQRCode.addClass("hide");
                _this.$linkHideQRCode.removeClass("hide");
            });
            _this.$linkHideQRCode.click(function () {
                _this.$imgQRCode.addClass("hide");
                _this.$logoLetstalk.removeClass("hide");
                _this.$linkHideQRCode.addClass("hide");
                _this.$linkShowQRCode.removeClass("hide");
            });
        };
        _this.addAccountEvent = function (e) {
            e.preventDefault();
            var letstalkId = _this.$txtAccount.val().trim();
            _this.$txtAccount.val(letstalkId);
            if (letstalkId.length === 0) {
                _this.$txtAccount.focus();
                alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 1, Message: _this.resources.accountIsRequired }));
                return;
            }
            else {
                _this.$addAccountMessage.html("");
            }
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/AddAccount",
                data: { letstalkId: letstalkId },
                dataType: "text",
                success: _this.onAddAccountSuccess,
                complete: _this.onComplete,
            });
        };
        _this.onAddAccountSuccess = function (data) {
            var result = _this.safelyParseJSON(data);
            if (result && result.IsNotOk) {
                _this.$txtAccount.focus();
                if (result.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(result.Message);
                    return;
                }
                if (result.IsMessengerAccountDoesNotExist) {
                    alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 2, Message: _this.resources.accountDoesNotExist }));
                    return;
                }
                if (result.IsMessengerAccountHasBeenRegisteredByAnotherCustomer) {
                    alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 5, Message: _this.resources.accountHasAlreadyBeenRegistered }));
                    return;
                }
                if (result.IsOverMaxAddAccountTimes) {
                    alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 6, Message: _this.resources.overMaxAddAccountTimes }));
                    return;
                }
                alpha.showMessage(_this.$addAccountMessage, new Result({ Code: result.Code, Message: result.Message }));
                return;
            }
            _this.$btnAdd.hide();
            _this.$btnUnsubscribe.show();
            _this.$txtAccount.attr("disabled", "disabled");
            _this.$divLinkTooltip.remove();
            _this.$registerLetstalkPanel.append(data);
            _this.updateExpiredTime();
            _this.verifyAccount.init();
        };
        _this.unsubscribeEvent = function (e) {
            e.preventDefault();
            var self = _this;
            _this.topWindow.bootboxConfirm(_this.resources.unsubscribeWarning, undefined, function () {
                _this.$.blockUI();
                _this.$.ajax({
                    type: "POST",
                    url: alpha.urls.mainRootPath + "ChatBot/Unsubscribe",
                    dataType: "json",
                    success: self.onUnSubscribeSuccess,
                    complete: self.onComplete
                });
            });
        };
        _this.onUnSubscribeSuccess = function (data) {
            if (data && data.IsNotOk) {
                if (data.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(data.Message);
                    return;
                }
                var result = new Result(data);
                alpha.showMessage(_this.$addAccountMessage, result);
                return;
            }
            parent.window.location.href = parent.window.location.href;
        };
        _this.verifyAccount = new VerifyAccount($, topWindow);
        return _this;
    }
    return AddAccount;
}(BaseChatBot));
$(function () {
    var addAccount = new AddAccount($, window.top);
    addAccount.init();
});
//# sourceMappingURL=add-account.js.map