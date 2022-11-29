/// <reference path="../core.ts" />
"use strict";

class SuggestTelegramOtp {
    $: JQueryStatic;
    $suggestOtpMessage: JQuery;

    isShowSuggestEnableOtpPopup;
    isShowedTryItNow;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.isShowSuggestEnableOtpPopup = [
            "SuggestEnableTelegramOtp",
            this.$("#userId").val(),
            this.$("#subUserId").val()
        ].join("_");

        this.isShowedTryItNow = "IsShowedTryItNow";

        this.cachedDOMElements();

        if (!this.getLocalStorageValue(this.isShowSuggestEnableOtpPopup) && this.getLocalStorageValue(this.isShowedTryItNow)) {
            this.showSuggestEnableTelegramOtp();
            this.setLocalStorageValue(true);
        }

        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$suggestOtpMessage = this.$("#suggest-otp-message");
    }

    registerEvents = () => {
        this.$("#enableTelegramOtp").click(this.enableTelegramOtp);
        this.$("#closeTelegramOtpPopup").click(this.closeTelegramOtpPopup);
    }

    enableTelegramOtp = () => {
        const self = this;
        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/UpdateOtpFunction",
            data: { enabled: true },
            dataType: "json",
            success: (result) => {
                alpha.showMessage(this.$suggestOtpMessage, new Result({ Code: 0, Message: "Enable Telegram OTP successfully." }))
                setTimeout(() => {
                    parent.window.location.href = parent.window.location.href;
                }, 2000);
            },
            complete: function (jqXHR, textStatus) {
                self.$.unblockUI();
            }
        });
    }

    showSuggestEnableTelegramOtp() {
        const popup = this.$("#telegramotp-modal");
        popup.modal({ show: true, backdrop: false })
            .tinyDraggable({ exclude: "h3, ol, li, p, img, label, input[type='checkbox'], a, button" });
    }

    closeTelegramOtpPopup = () => {
        const popup = this.$("#telegramotp-modal");
        popup.modal("hide");
    }

    setLocalStorageValue(value: boolean) {
        alpha.localStorage.set(this.isShowSuggestEnableOtpPopup, value ? "1" : "0");
    }

    getLocalStorageValue(key: string) {
        return alpha.localStorage.get(key) === "1";
    }
}

$(function () {
    new SuggestTelegramOtp($).init();
});
