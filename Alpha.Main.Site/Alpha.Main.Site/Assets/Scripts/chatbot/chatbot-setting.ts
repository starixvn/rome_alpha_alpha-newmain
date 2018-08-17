/// <reference path="../core.ts" />

"use strict";

class ChatBotSetting extends BaseChatBot {
    // Language
    $language: JQuery;
    $languageSelectorContainer: JQuery;
    $changeLanguageLink: JQuery;
    $userId: JQuery;
    $userName: JQuery;
    $btnUnsubscribe: JQuery;
    $accountMessage: JQuery;
    $openReportExample: JQuery;
    $openNotificationExample: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        super($, topWindow)
    }

    resources = {
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value,
        updateLanguageFailed: (<HTMLInputElement>document.getElementById('UpdateLanguageFailed')).value,
        reportExample: (<HTMLInputElement>document.getElementById('ReportExample')).value,
        notificationExample: (<HTMLInputElement>document.getElementById('NotificationExample')).value
    }

    init = () => {
        alpha.detectResponsiveInformation();
        this.cachedDOMElements();
        this.initLanguage();
        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$language = this.$("#language");
        this.$accountMessage = this.$("#account-message");
        this.$languageSelectorContainer = this.$(".language-selector-container");
        this.$changeLanguageLink = this.$(".changelanguage-link");
        this.$userId = this.$("#UserId");
        this.$userName = this.$("#UserName");
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
        this.$openReportExample = this.$("#openReportExample");
        this.$openNotificationExample = this.$("#openNotificationExample");
    }

    registerEvents = () => {
        this.$(".function").click(this.updateFunction);
        this.$btnUnsubscribe.click(this.unsubscribeEvent);

        var self = this;
        this.$changeLanguageLink.on('click touchend', (e) => {
            self.changeLanguage(self.$(e.target).data("value"));
        });

        this.$openReportExample.on('click touchend', () => {
            this.openExample(this.resources.reportExample, "ChatBot/ReportExample", 380);
        });

        this.$openNotificationExample.on('click touchend', () => {
            this.openExample(this.resources.notificationExample, "ChatBot/NotificationExample", 336);
        });
    }

    openExample = (title: string, url: string, height: number) => {
        var language = this.$language.data("currentlanguage");
        var src = alpha.urls.mainRootPath + url + "?language=" + language;

        this.topWindow.bootbox.dialog({
            title: title,
            message: '<iframe height="' + height + 'px" width="100%" src="' + src + '"></iframe>'
        });

        var dialog = this.topWindow.$(".bootbox .modal-dialog");
        dialog.width(374);
        dialog.css({
            left: this.topWindow.$(this.topWindow.window).width() / 2 - dialog.width() / 2,
            top: 100
        });
    }

    initLanguage = () => {
        var currentLanguage = this.$language.data("currentlanguage");
        var language = this.$(".list-language ." + currentLanguage + " > a").html();

        this.$languageSelectorContainer.prepend(language);
    }

    updateFunction = (e: JQueryEventObject) => {
        var userId = this.$userId.val();
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
            data: { command: { userId: userId, functionId: functionId, functionName: functionName, command: command, isEnable: isEnable } },
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
        var userId = self.$userId.val();
        var userName = self.$userName.val();
        this.topWindow.bootboxConfirm(this.resources.unsubscribeWarning, undefined, () => {
            this.$.blockUI();
            this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/Unsubscribe",
                data: { userId: userId, messengerUserName: userName },
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
        var userId = this.$userId.val();
        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/ChangeLanguage",
            data: { userId: userId, language: lang },
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
}

$(function () {
    var chatBotSetting = new ChatBotSetting($, window.top);
    chatBotSetting.init();
});