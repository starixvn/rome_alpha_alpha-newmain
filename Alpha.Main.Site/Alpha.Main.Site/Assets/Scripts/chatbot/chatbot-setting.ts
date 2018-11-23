/// <reference path="../core.ts" />

"use strict";

class ChatBotSetting extends BaseChatBot {
    // Language
    $language: JQuery;
    $languageSelectorContainer: JQuery;
    $changeLanguageLink: JQuery;
    $btnUnsubscribe: JQuery;
    $accountMessage: JQuery;
    $selfdestructionPeriod: JQuery;
    $selfdestructionPeriodContainer: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        super($, topWindow)
    }

    resources = {
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value,
        updateLanguageFailed: (<HTMLInputElement>document.getElementById('UpdateLanguageFailed')).value,
        updateMessageSelfDestructMessageFailedMessage: (<HTMLInputElement>document.getElementById('UpdateMessageSelfDestructMessageFailedMessage')).value
    }

    init = () => {
        alpha.detectResponsiveInformation();
        this.cachedDOMElements();
        this.initLanguage();
        this.registerEvents();
        this.cacheDOMAndRegisterFunctionExample(() => this.$language.data("currentlanguage"));
    }

    cachedDOMElements = () => {
        this.$language = this.$("#language");
        this.$accountMessage = this.$("#account-message");
        this.$languageSelectorContainer = this.$(".language-selector-container");
        this.$changeLanguageLink = this.$(".changelanguage-link");
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
        this.$selfdestructionPeriod = this.$("#period");
    }

    registerEvents = () => {
        this.$(".function").click(this.updateFunction);
        this.$btnUnsubscribe.click(this.unsubscribeEvent);

        var self = this;
        this.$changeLanguageLink.on('click touchend', (e) => {
            self.changeLanguage(self.$(e.target).data("value"));
        });
        this.$selfdestructionPeriod.change(this.changeSelfdestructionPeriod);
    }

    initLanguage = () => {
        var currentLanguage = this.$language.data("currentlanguage");
        var language = this.$(".list-language ." + currentLanguage + " > a").html();

        this.$languageSelectorContainer.prepend(language);
    }

    updateFunction = (e: JQueryEventObject) => {
        var checkbox = this.$(e.target);
        var functionId = checkbox.attr("data-functionId");
        var functionName = checkbox.attr("data-functionName");
        var command = checkbox.attr("data-command");
        var isEnable = checkbox.is(":checked");
        var self = this;
        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/UpdateFunction",
            data: { command: { functionId: functionId, functionName: functionName, command: command, isEnable: isEnable } },
            dataType: "json",
            success: (result) => {
                if (result && result.IsNotOk) {
                    if (result.IsNotAuthorized) {
                        self.topWindow.bootboxAlert(result.Message);
                    }
                    checkbox.prop("checked", !isEnable);
                }
            },
            complete: this.onComplete,
        });
    }

    unsubscribeEvent = (e: any) => {
        e.preventDefault();
        var self = this;
        this.topWindow.bootboxConfirm(this.resources.unsubscribeWarning, undefined, () => {
            this.$.blockUI();
            this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/Unsubscribe",
                dataType: "json",
                success: self.onUnSubscribeSuccess,
                complete: self.onComplete
            });
        });
    }

    onUnSubscribeSuccess = (data: any) => {
        if (data && data.IsNotOk) {
            if (data.IsNotAuthorized) {
                this.topWindow.bootboxAlert(data.Message);
                return;
            }

            var result = new Result(data);
            alpha.showMessage(this.$accountMessage, result);
            return;
        }

        parent.window.location.href = parent.window.location.href;
    }

    changeLanguage = (lang) => {
        this.$.blockUI();
        var currentLanguage = this.$language.data("currentlanguage");

        if (lang == currentLanguage) {
            this.onComplete();
            return;
        }

        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/ChangeLanguage",
            data: { language: lang },
            success: this.onChangeLanguageSuccess,
            complete: this.onComplete
        });
    }

    onChangeLanguageSuccess = (result: any) => {
        if (result) {
            if (result.IsOk) {
                parent.window.location.href = parent.window.location.href;
            } else {
                if (result.IsNotAuthorized) {
                    this.topWindow.bootboxAlert(result.Message);
                    return;
                }

                alpha.showMessage(this.$accountMessage, new Result({ Code: 1, Message: this.resources.updateLanguageFailed }));
            }
        }
    }

    changeSelfdestructionPeriod = (e: any) => {
        e.preventDefault();
        this.$.blockUI();
        var minutes = this.$selfdestructionPeriod.find('option:selected').val();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/UpdateSelfdestructionPeriod",
            data: { minutes: minutes },
            success: this.onChangeSelfdestructionPeriodSuccess,
            complete: this.onComplete
        });
    }

    onChangeSelfdestructionPeriodSuccess = (result: any) => {
        if (result && result.IsNotOk) {
            if (result.IsNotAuthorized) {
                this.topWindow.bootboxAlert(result.Message);
                return;
            }
            alpha.showMessage(
                this.$accountMessage,
                new Result({ Code: 1, Message: this.resources.updateMessageSelfDestructMessageFailedMessage }));
        }
    }
}

$(function () {
    var chatBotSetting = new ChatBotSetting($, window.top);
    chatBotSetting.init();
});