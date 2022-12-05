/// <reference path="../core.ts" />
"use strict";

declare var Clipboard: any;

class TelegramChatBotSetting extends BaseTelegramChatBot {
    clipboard: any;

    // Language
    $language: JQuery;
    $languageSelectorContainer: JQuery;
    $changeLanguageLink: JQuery;
    $btnUnsubscribe: JQuery;
    $accountMessage: JQuery;
    $copyIcon: JQuery;

    $userId: JQuery;
    $container: JQuery;
    $overlay: JQuery;

    HAS_UNLINKED_ERR_CODE = 102;
    TELEGRAM_OTP_FUNCTION_ID = 15;
    isFirstLoad = true;

    constructor($: JQueryStatic, topWindow: any, clipboard: any) {
        super($, topWindow);
        this.clipboard = clipboard;
    }

    resources = {
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value,
        updateLanguageFailed: (<HTMLInputElement>document.getElementById('UpdateLanguageFailed')).value,
        hasUnlinkedMessage: (<HTMLInputElement>document.getElementById('HasUnlinkedMessage')).value
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
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
        this.$copyIcon = this.$("#copy-icon");
        this.$userId = this.$("#UserId");
        this.$overlay = this.$("#overlay");
        this.$container = this.$("#container");
    }

    showHighlight = () => {
        this.$container.addClass("chatbot-linked-telegram");
        this.$(window).scrollTop(900);
        this.setLocalStorageValue("IsShowedTryItNow", true);
    }

    registerEvents = () => {
        this.$(".function").click(this.updateFunction);
        this.$btnUnsubscribe.click(this.unsubscribeEvent);

        var self = this;
        this.$changeLanguageLink.on('click touchend', (e) => {
            self.changeLanguage(self.$(e.target).data("value"));
        });

        if (window.document.queryCommandSupported
            && window.document.queryCommandSupported("copy")) {
            this.clipboard
                .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                })
                .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
        }
        else {
            this.$copyIcon.remove();
        }

        this.isFirstLoad = !this.getLocalStorageValue("IsShowedTryItNow");
        if (this.isFirstLoad) {
            this.showHighlight();
            this.$overlay.click(() => {
                this.$container.removeClass("chatbot-linked-telegram");
            });
        }
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
        var type = checkbox.attr("data-type");
        var enabled = checkbox.is(":checked");

        var self = this;
        this.$.blockUI();

        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "TelegramChatBot/UpdateFunction",
            data: { functionId, functionName, type, enabled },
            dataType: "json",
            success: (result) => {
                if (result && result.IsNotOk) {
                    if (result.IsNotAuthorized) {
                        self.topWindow.bootboxAlert(result.Message);
                        return;
                    }

                    if (result.Code == this.HAS_UNLINKED_ERR_CODE) {
                        this.showHasUnlinkedMessage();
                        return;
                    }

                    checkbox.prop("checked", !enabled);
                }

                if (functionId == this.TELEGRAM_OTP_FUNCTION_ID.toString()) {
                    setTimeout(() => {
                        parent.window.location.href = parent.window.location.href;
                    }, 2000);
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
                url: alpha.urls.mainRootPath + "TelegramChatBot/Unsubscribe",
                dataType: "json",
                success: self.onUnSubscribeSuccess,
                complete: self.onComplete
            });
        });
    }

    onUnSubscribeSuccess = (result: any) => {
        if (result && result.IsNotOk) {
            if (result.IsNotAuthorized) {
                this.topWindow.bootboxAlert(result.Message);
                return;
            }

            if (result.Code == this.HAS_UNLINKED_ERR_CODE) {
                this.showHasUnlinkedMessage();
                return;
            }

            alpha.showMessage(this.$accountMessage, new Result(result));

            return;
        }

        this.reloadPage();
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
            url: alpha.urls.mainRootPath + "TelegramChatBot/UpdateLanguage",
            data: { language: lang },
            success: this.onChangeLanguageSuccess,
            complete: this.onComplete
        });
    }

    onChangeLanguageSuccess = (result: any) => {
        if (result) {
            if (result.IsOk) {
                this.reloadPage();
            } else {
                if (result.IsNotAuthorized) {
                    this.topWindow.bootboxAlert(result.Message);
                    return;
                }

                if (result.Code == this.HAS_UNLINKED_ERR_CODE) {
                    this.showHasUnlinkedMessage();
                    return;
                }

                alpha.showMessage(this.$accountMessage, new Result({ Code: 1, Message: this.resources.updateLanguageFailed }));
            }
        }
    }

    showHasUnlinkedMessage = () => {
        this.topWindow.bootboxAlert(this.resources.hasUnlinkedMessage, undefined, () => this.reloadPage());
    }

    setLocalStorageValue(key: string, value: boolean) {
        alpha.localStorage.set(key, value ? "1" : "0");
    }

    getLocalStorageValue(key: string) {
        return alpha.localStorage.get(key) === "1";
    }
}

$(function () {
    var telegramChatBotSetting = new TelegramChatBotSetting($, window.top, new Clipboard('.icon-content-copy'));
    telegramChatBotSetting.init();
});