/// <reference path="../core.ts" />

class AddAccount extends BaseChatBot {
    // Add Account
    $registerLetstalkPanel: JQuery;
    $txtAccount: JQuery;
    $addAccountMessage: JQuery;
    $btnAdd: JQuery;
    $btnUnsubscribe: JQuery;

    $logoLetstalk: JQuery;
    $imgQRCode: JQuery;
    $linkShowQRCode: JQuery;
    $linkHideQRCode: JQuery;
    $divLinkTooltip: JQuery;

    verifyAccount: VerifyAccount;

    constructor($: JQueryStatic, topWindow: any) {
        super($, topWindow);
        this.verifyAccount = new VerifyAccount($, topWindow);
    }

    resources = {
        accountDoesNotExist: (<HTMLInputElement>document.getElementById('AccountDoesNotExist')).value,
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value,
        overMaxAddAccountTimes: (<HTMLInputElement>document.getElementById('OverMaxAddAccountTimes')).value,
        accountHasAlreadyBeenRegistered: (<HTMLInputElement>document.getElementById('AccountHasAlreadyBeenRegistered')).value,
        accountIsRequired: (<HTMLInputElement>document.getElementById('AccountIsRequired')).value,
        language: (<HTMLInputElement>document.getElementById('Language')).value
    }

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();

        var isVerifying = this.convertAttributeToBool(this.$("#IsVerifying").val());
        if (isVerifying) {
            this.verifyAccount.init();
        }

        this.cacheDOMAndRegisterFunctionExample(() => this.resources.language);
    }

    cachedDOMElements = () => {
        this.$registerLetstalkPanel = this.$("#registerLetstalkPanel");
        this.$txtAccount = this.$("#txtAccount");
        this.$addAccountMessage = this.$("#add-account-message");
        this.$btnAdd = this.$("#btnAdd");
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");

        this.$logoLetstalk = this.$("#logoLetstalk");
        this.$imgQRCode = this.$("#imgQRCode");
        this.$linkShowQRCode = this.$("#showQRCode");
        this.$linkHideQRCode = this.$("#hideQRCode");
        this.$divLinkTooltip = this.$("#divLinkTooltip");
    }

    registerEvents = () => {
        this.$txtAccount.keypress((e) => {
            if (e.which === 32) {
                return false;
            }

            if (e.keyCode === 13) {
                this.$btnAdd.click();
            }
        });

        this.$btnAdd.click(this.addAccountEvent);
        this.$btnUnsubscribe.click(this.unsubscribeEvent);

        this.$linkShowQRCode.click(() => {
            this.$logoLetstalk.addClass("hide");
            this.$imgQRCode.removeClass("hide");
            this.$linkShowQRCode.addClass("hide");
            this.$linkHideQRCode.removeClass("hide");
        });

        this.$linkHideQRCode.click(() => {
            this.$imgQRCode.addClass("hide");
            this.$logoLetstalk.removeClass("hide");
            this.$linkHideQRCode.addClass("hide");
            this.$linkShowQRCode.removeClass("hide");
        });
    }

    addAccountEvent = (e: any) => {
        e.preventDefault();

        var letstalkId = this.$txtAccount.val().trim();
        this.$txtAccount.val(letstalkId);

        if (letstalkId.length === 0) {
            this.$txtAccount.focus();
            alpha.showMessage(this.$addAccountMessage, new Result({ Code: 1, Message: this.resources.accountIsRequired }));
            return;
        } else {
            this.$addAccountMessage.html("");
        }

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/AddAccount",
            data: { letstalkId: letstalkId },
            dataType: "text",
            success: this.onAddAccountSuccess,
            complete: this.onComplete,
        });
    }

    onAddAccountSuccess = (data: any) => {
        var result = this.safelyParseJSON(data);

        if (result && result.IsNotOk) {
            this.$txtAccount.focus();

            if (result.IsNotAuthorized) {
                this.topWindow.bootboxAlert(result.Message);
                return;
            }

            if (result.IsMessengerAccountDoesNotExist) {
                alpha.showMessage(this.$addAccountMessage, new Result({ Code: 2, Message: this.resources.accountDoesNotExist }));
                return
            }

            if (result.IsMessengerAccountHasBeenRegisteredByAnotherCustomer) {
                alpha.showMessage(this.$addAccountMessage, new Result({ Code: 5, Message: this.resources.accountHasAlreadyBeenRegistered }));
                return
            }

            if (result.IsOverMaxAddAccountTimes) {
                alpha.showMessage(this.$addAccountMessage, new Result({ Code: 6, Message: this.resources.overMaxAddAccountTimes }));
                return
            }

            alpha.showMessage(this.$addAccountMessage, new Result({ Code: result.Code, Message: result.Message }));
            return;
        }

        this.$btnAdd.hide();
        this.$btnUnsubscribe.show();
        this.$txtAccount.attr("disabled", "disabled");
        this.$divLinkTooltip.remove();

        this.$registerLetstalkPanel.append(data);

        this.updateExpiredTime();
        this.verifyAccount.init();
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
            alpha.showMessage(this.$addAccountMessage, result);
            return;
        }
        parent.window.location.href = parent.window.location.href;
    }
}

$(function () {
    const addAccount = new AddAccount($, window.top);
    addAccount.init();
});