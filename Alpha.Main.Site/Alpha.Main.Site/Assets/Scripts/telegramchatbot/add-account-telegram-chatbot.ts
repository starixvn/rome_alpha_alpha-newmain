/// <reference path="../core.ts" />

class AddAccountTelegramChatBot extends BaseTelegramChatBot {
    // Add Account
    $registerTelegramPanel: JQuery;
    $addAccountMessage: JQuery;
    $btnGenQRCode: JQuery;

    $logoTelegram: JQuery;
    $imgQRCode: JQuery;
    $linkShowQRCode: JQuery;
    $linkHideQRCode: JQuery;

    verifyAccountTelegramChatbot: VerifyAccountTelegramChatbot;

    constructor($: JQueryStatic, topWindow: any) {
        super($, topWindow);
        this.verifyAccountTelegramChatbot = new VerifyAccountTelegramChatbot($, topWindow);
    }

    resources = {
        language: (<HTMLInputElement>document.getElementById('Language')).value
    }

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$registerTelegramPanel = this.$("#registerTelegramPanel");
        this.$addAccountMessage = this.$("#add-account-message");
        this.$btnGenQRCode = this.$("#btnGenQRCode");

        this.$logoTelegram = this.$("#logoTelegram");
        this.$imgQRCode = this.$("#imgQRCode");
        this.$linkShowQRCode = this.$("#showQRCode");
        this.$linkHideQRCode = this.$("#hideQRCode");
    }

    registerEvents = () => {
        this.$btnGenQRCode.click(this.genQRCodeEvent);

        this.$linkShowQRCode.click(() => {
            this.$logoTelegram.addClass("hide");
            this.$imgQRCode.removeClass("hide");
            this.$linkShowQRCode.addClass("hide");
            this.$linkHideQRCode.removeClass("hide");
        });

        this.$linkHideQRCode.click(() => {
            this.$imgQRCode.addClass("hide");
            this.$logoTelegram.removeClass("hide");
            this.$linkHideQRCode.addClass("hide");
            this.$linkShowQRCode.removeClass("hide");
        });
    }

    genQRCodeEvent = (e: any) => {
        e.preventDefault();

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "TelegramChatBot/GenerateQRCode",
            dataType: "text",
            success: this.onGenQRCodeSuccess,
            complete: this.onComplete
        })

    }

    onGenQRCodeSuccess = (data: any) => {
        this.$btnGenQRCode.attr("disabled", "disabled");

        this.$registerTelegramPanel.append(data);

        this.verifyAccountTelegramChatbot.init();
    }
}

$(function () {
    const addAccountTelegramChatbot = new AddAccountTelegramChatBot($, window.top);
    addAccountTelegramChatbot.init();
});