/// <reference path="../../core.ts" />

class ChangeOtpTelegramStatus {
    $: JQueryStatic;
    topWindow: any;
    $btnUnsubscribe: JQuery;
    $cbkEnableOtp: JQuery;

    isEnableTelegramOtp;
    isShowRedirectToTelegramPage;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.isShowRedirectToTelegramPage = "IsShowRedirectToTelegramChatPage";
        this.isEnableTelegramOtp = "IsEnableTelegramOtp";
        this.cachedDOMElements();
        this.registerEvents();
        this.showPopupRedirectToTelegramPage();
    }

    resources = {
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value
    }

    cachedDOMElements = () => {
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
        this.$cbkEnableOtp = this.$(".cbkEnableOtp");
    }

    registerEvents = () => {
        this.$btnUnsubscribe.click(this.unsubscribeEvent);
        this.$cbkEnableOtp.click(this.updateFunction);
    }

    showPopupRedirectToTelegramPage = () => {
        const isEnableTelegramOtp = this.getLocalStorageValue(this.isEnableTelegramOtp);
        const isShowRedirectToTelegramPage = this.getLocalStorageValue(this.isShowRedirectToTelegramPage);
        if (isEnableTelegramOtp && !isShowRedirectToTelegramPage) {
            const self = this;
            const redirectMess = "OTP Status is enabled. You can get OTP via Telegram Chatbot now. And please redirect to the Telegram page to view more information and setting of the Telegram Chatbot.";
            this.topWindow.bootboxConfirm(redirectMess, undefined, () => {
                window.location.href = alpha.urls.mainRootPath + "TelegramChatBot/Setup";
                self.setLocalStorageValue(this.isShowRedirectToTelegramPage, true);
            });
        }
    }

    unsubscribeEvent = (e: any) => {
        e.preventDefault();
        this.topWindow.bootboxConfirm(this.resources.unsubscribeWarning, undefined, () => {
            const self = this;
            this.$.blockUI();
            this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "TelegramChatBot/Unsubscribe",
                dataType: "text",
                success: function (data, textStatus, jqXHR) {
                    parent.window.location.href = parent.window.location.href;
                },
                complete: function (jqXHR, textStatus) {
                    self.$.unblockUI();
                }
            });
        });
    }

    updateFunction = (e: JQueryEventObject) => {
        var checkbox = this.$(e.target);
        var enabled = checkbox.is(":checked");
        this.$.blockUI();
        var self = this;
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/UpdateOtpFunction",
            data: { enabled },
            dataType: "json",
            success: (result) => {
                if (!self.getLocalStorageValue(self.isShowRedirectToTelegramPage)) {
                    self.setLocalStorageValue(self.isEnableTelegramOtp, enabled);
                }

                setTimeout(() => {
                    parent.window.location.href = parent.window.location.href;
                }, 2000);
            },
            complete: function (jqXHR, textStatus) {
                self.$.unblockUI();
            }
        });
    }

    setLocalStorageValue(key: string, value: boolean) {
        alpha.localStorage.set(key, value ? "1" : "0");
    }

    getLocalStorageValue(key: string) {
        return alpha.localStorage.get(key) === "1";
    }
}

$(function () {
    const changeOtpTelegramStatus = new ChangeOtpTelegramStatus($, window.top);
    changeOtpTelegramStatus.init();
});