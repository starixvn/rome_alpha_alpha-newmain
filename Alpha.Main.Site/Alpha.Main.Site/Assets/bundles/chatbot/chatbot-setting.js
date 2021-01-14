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
var ChatBotSetting = /** @class */ (function (_super) {
    __extends(ChatBotSetting, _super);
    function ChatBotSetting($, topWindow) {
        var _this = _super.call(this, $, topWindow) || this;
        _this.resources = {
            unsubscribeWarning: document.getElementById('UnsubscribeWarning').value,
            updateLanguageFailed: document.getElementById('UpdateLanguageFailed').value,
            updateMessageSelfDestructMessageFailedMessage: document.getElementById('UpdateMessageSelfDestructMessageFailedMessage').value
        };
        _this.init = function () {
            alpha.detectResponsiveInformation();
            _this.cachedDOMElements();
            _this.initLanguage();
            _this.registerEvents();
            _this.cacheDOMAndRegisterFunctionExample(function () { return _this.$language.data("currentlanguage"); });
        };
        _this.cachedDOMElements = function () {
            _this.$language = _this.$("#language");
            _this.$accountMessage = _this.$("#account-message");
            _this.$languageSelectorContainer = _this.$(".language-selector-container");
            _this.$changeLanguageLink = _this.$(".changelanguage-link");
            _this.$btnUnsubscribe = _this.$("#btnUnsubscribe");
            _this.$selfdestructionPeriod = _this.$("#period");
        };
        _this.registerEvents = function () {
            _this.$(".function").click(_this.updateFunction);
            _this.$btnUnsubscribe.click(_this.unsubscribeEvent);
            var self = _this;
            _this.$changeLanguageLink.on('click touchend', function (e) {
                self.changeLanguage(self.$(e.target).data("value"));
            });
            _this.$selfdestructionPeriod.change(_this.changeSelfdestructionPeriod);
        };
        _this.initLanguage = function () {
            var currentLanguage = _this.$language.data("currentlanguage");
            var language = _this.$(".list-language ." + currentLanguage + " > a").html();
            _this.$languageSelectorContainer.prepend(language);
        };
        _this.updateFunction = function (e) {
            var checkbox = _this.$(e.target);
            var functionId = checkbox.attr("data-functionId");
            var functionName = checkbox.attr("data-functionName");
            var command = checkbox.attr("data-command");
            var isEnable = checkbox.is(":checked");
            var self = _this;
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/UpdateFunction",
                data: { command: { functionId: functionId, functionName: functionName, command: command, isEnable: isEnable } },
                dataType: "json",
                success: function (result) {
                    if (result && result.IsNotOk) {
                        if (result.IsNotAuthorized) {
                            self.topWindow.bootboxAlert(result.Message);
                        }
                        checkbox.prop("checked", !isEnable);
                    }
                },
                complete: _this.onComplete,
            });
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
                alpha.showMessage(_this.$accountMessage, result);
                return;
            }
            parent.window.location.href = parent.window.location.href;
        };
        _this.changeLanguage = function (lang) {
            _this.$.blockUI();
            var currentLanguage = _this.$language.data("currentlanguage");
            if (lang == currentLanguage) {
                _this.onComplete();
                return;
            }
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/ChangeLanguage",
                data: { language: lang },
                success: _this.onChangeLanguageSuccess,
                complete: _this.onComplete
            });
        };
        _this.onChangeLanguageSuccess = function (result) {
            if (result) {
                if (result.IsOk) {
                    parent.window.location.href = parent.window.location.href;
                }
                else {
                    if (result.IsNotAuthorized) {
                        _this.topWindow.bootboxAlert(result.Message);
                        return;
                    }
                    alpha.showMessage(_this.$accountMessage, new Result({ Code: 1, Message: _this.resources.updateLanguageFailed }));
                }
            }
        };
        _this.changeSelfdestructionPeriod = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            var minutes = _this.$selfdestructionPeriod.find('option:selected').val();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/UpdateSelfdestructionPeriod",
                data: { minutes: minutes },
                success: _this.onChangeSelfdestructionPeriodSuccess,
                complete: _this.onComplete
            });
        };
        _this.onChangeSelfdestructionPeriodSuccess = function (result) {
            if (result && result.IsNotOk) {
                if (result.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(result.Message);
                    return;
                }
                alpha.showMessage(_this.$accountMessage, new Result({ Code: 1, Message: _this.resources.updateMessageSelfDestructMessageFailedMessage }));
            }
        };
        return _this;
    }
    return ChatBotSetting;
}(BaseChatBot));
$(function () {
    var chatBotSetting = new ChatBotSetting($, window.top);
    chatBotSetting.init();
});
//# sourceMappingURL=chatbot-setting.js.map