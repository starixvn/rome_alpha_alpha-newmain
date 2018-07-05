/// <reference path="../core.ts" />

"use strict";

class ChatBot {
    $: JQueryStatic;
    bootbox: any;

    $registerLetstalkPanel: JQuery;
    $verificationForm: JQuery;
    $txtAccount: JQuery;
    $txtVerificationCode: JQuery;
    $addAccountMessage: JQuery;
    $verifcationCodeMessage: JQuery;
    $btnAdd: JQuery;
    $btnUnsubscribe: JQuery;
    $btnVerify: JQuery;
    $btnResend: JQuery;

    resources = {
        thisFieldRequired: (<HTMLInputElement>document.getElementById('ThisFieldRequired')).value,
        verificationCodeIsValidOrExpired: (<HTMLInputElement>document.getElementById('VerificationCodeIsValidOrExpired')).value,
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value
    }

    constructor($: any, bootbox: any) {
        this.$ = $;
        this.bootbox = bootbox;
    }

    init = () => {
        this.cachDOMElements();
        this.registerEvents();
    }

    cachDOMElements = () => {
        this.$registerLetstalkPanel = this.$("#registerLetstalkPanel");
        this.$txtAccount = this.$("#txtAccount");
        this.$addAccountMessage = this.$("#add-account-message");
        this.$btnAdd = this.$("#btnAdd");
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
    }

    registerEvents = () => {
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
        }

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/AddAccount",
            data: { account: account },
            dataType: "text",
            success: this.onAddAccountSuccess,
            complete: this.onComplete,
        });
    }

    onAddAccountSuccess = (data: any) => {
        this.$btnAdd.hide();
        this.$btnUnsubscribe.show();
        this.$registerLetstalkPanel.append(data);

        this.$verificationForm = this.$("#verification-form");
        this.$txtVerificationCode = this.$("#txtVerificationCode");
        this.$verifcationCodeMessage = this.$("#verification-code-message");
        this.$btnVerify = this.$("#btnVerify");
        this.$btnResend = this.$("#btnResend");
        this.$verificationForm.submit(this.verifyCodeEvent);
    }

    unsubscribeEvent = (e: any) => {
        var self = this;
        this.bootbox.confirm(this.resources.unsubscribeWarning, (ok) => {
        });
    }

    resendEvent = (e: any) => {
    }

    verifyCodeEvent = (e: any) => {
        e.preventDefault();

        var verificationCode = this.$txtVerificationCode.val();
        if (verificationCode.length === 0) {
            this.$txtVerificationCode.focus();
            alpha.showMessage(this.$verifcationCodeMessage, new Result({ Code: 1, Message: this.resources.thisFieldRequired }));
            return;
        }
    }

    onComplete = () => {
        this.$.unblockUI();
    }
}

var chatBot = new ChatBot($, bootbox);

$(function () {
    chatBot.init();
});