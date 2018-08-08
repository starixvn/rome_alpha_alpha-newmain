/// <reference path="../core.ts" />

class AddAccount extends BaseChatBot {
    $: JQueryStatic

    // Add Account
    $registerLetstalkPanel: JQuery;
    $txtAccount: JQuery;
    $addAccountMessage: JQuery;
    $btnAdd: JQuery;
    $btnUnsubscribe: JQuery;
    $userId: JQuery;

    verifyAccount: VerifyAccount;

    constructor($: JQueryStatic, bootbox: any) {
        super($, bootbox);
        this.$ = $;
        this.verifyAccount = new VerifyAccount($, bootbox);
    }

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();

        var isVerifying = this.convertAttributeToBool(this.$("#IsVerifying").val());
        if (isVerifying) {
            this.$("#countdown").text(0);
            this.verifyAccount.init();
        }
    }

    cachedDOMElements = () => {
        this.$registerLetstalkPanel = this.$("#registerLetstalkPanel");
        this.$txtAccount = this.$("#txtAccount");
        this.$addAccountMessage = this.$("#add-account-message");
        this.$btnAdd = this.$("#btnAdd");
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
        this.$userId = this.$("#UserId");
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
    }

    addAccountEvent = (e: any) => {
        e.preventDefault();

        var account = this.$txtAccount.val();
        if (account.length === 0) {
            this.$txtAccount.focus();
            alpha.showMessage(this.$addAccountMessage, new Result({ Code: 1, Message: this.resources.thisFieldRequired }));
            return;
        } else {
            this.$addAccountMessage.html("");
        }

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/AddAccount",
            data: { messengerAccount: account },
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
                this.bootbox.alert(result.Message);
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

        this.$registerLetstalkPanel.append(data);
        this.$userId = this.$("#UserId");
        this.verifyAccount.init();
    }

    unsubscribeEvent = (e: any) => {
        e.preventDefault();
        var self = this;
        var userId = self.$userId.val();
        var userName = self.$txtAccount.val();
        this.bootbox.confirm(this.resources.unsubscribeWarning, (ok) => {
            if (ok) {
                this.$.blockUI();
                this.$.ajax({
                    type: "POST",
                    url: alpha.urls.mainRootPath + "ChatBot/Unsubscribe",
                    data: { userId: userId, messengerUserName: userName },
                    dataType: "json",
                    success: self.onUnSubscribeSuccess,
                    complete: self.onComplete
                });
            }
        });
    }

    onUnSubscribeSuccess = (data: any) => {
        if (data && data.IsNotOk) {
            if (data.IsNotAuthorized) {
                this.bootbox.alert(data.Message);
                return;
            }

            var result = new Result(data);
            alpha.showMessage(this.$addAccountMessage, result);
            return;
        }
        setTimeout(() => {
            parent.window.location.href = parent.window.location.href;
        }, 2000);
    }
}

$(function () {
    const addAccount = new AddAccount($, bootbox);
    addAccount.init();
});