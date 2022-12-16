/// <reference path="../core.ts" />
"use strict";

class SuggestTelegramOtp {
    $: JQueryStatic;
    $suggestOtpMessage: JQuery;

    isShowedSuggestEnableOtpPopup;
    isShowingSuggestEnableOtpPopup;
    enableOTPSuccessfullMessage;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.isShowedSuggestEnableOtpPopup = "IsShowedSuggestEnableOtpPopup";
        this.isShowingSuggestEnableOtpPopup = "IsShowingSuggestEnableOtpPopup";

        this.cachedDOMElements();

        if (!this.getLocalStorageValue(this.isShowedSuggestEnableOtpPopup)) {
            this.showSuggestEnableTelegramOtp();
            this.setLocalStorageValue(this.isShowedSuggestEnableOtpPopup, true);
        }

        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$suggestOtpMessage = this.$("#suggest-otp-message");
        this.enableOTPSuccessfullMessage = this.$("#EnabledOTPSuccessMessage").val();
    }

    registerEvents = () => {
        this.$("#enableTelegramOtp").click(this.enableTelegramOtp);
        this.$("#closeTelegramOtpPopup").click(this.closeTelegramOtpPopup);
        this.$("#telegramotp-modal").find(".icon-close").click(this.handleCloseSuggestPopup);
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
                alpha.showMessage(this.$suggestOtpMessage, new Result({ Code: 0, Message: this.enableOTPSuccessfullMessage }))
                setTimeout(() => {
                    parent.window.location.href = parent.window.location.href;
                    this.setLocalStorageValue(this.isShowingSuggestEnableOtpPopup, false);
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
        this.setLocalStorageValue(this.isShowingSuggestEnableOtpPopup, true);
    }

    handleCloseSuggestPopup = () => {
        this.setLocalStorageValue(this.isShowingSuggestEnableOtpPopup, false);
    }

    closeTelegramOtpPopup = () => {
        const popup = this.$("#telegramotp-modal");
        popup.modal("hide");
        this.handleCloseSuggestPopup();
    }

    setLocalStorageValue(key: string, value: boolean) {
        alpha.localStorage.set(key, value ? "1" : "0");
    }

    getLocalStorageValue(key: string) {
        return alpha.localStorage.get(key) === "1";
    }
}

$(function () {
    new SuggestTelegramOtp($).init();
});
