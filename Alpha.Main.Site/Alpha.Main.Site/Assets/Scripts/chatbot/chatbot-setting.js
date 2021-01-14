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