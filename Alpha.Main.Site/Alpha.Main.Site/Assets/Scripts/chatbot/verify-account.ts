/// <reference path="../core.ts" />
"use strict";

class VerifyAccount extends BaseChatBot {
    $: JQueryStatic;

    // Verify Account
    $verificationSection: JQuery;
    $verificationForm: JQuery;
    $txtVerificationCode: JQuery;
    $verificationMessage: JQuery;
    $userId: JQuery;
    $messengerAccount: JQuery;
    $btnVerify: JQuery;
    $btnResend: JQuery;

    // Captcha
    $txtCaptcha: JQuery;
    $captchaMessage: JQuery;
    $captcha: JQuery;
    $captchaRefreshIcon: JQuery;
    $modelImageElement: JQuery;
    $modelTokenParameterName: JQuery;
    $modelTokenElement: JQuery;
    $modelRefreshUrl: JQuery;

    isShowCaptcha: boolean;

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$verificationSection = this.$("#verification-section");
        this.$verificationForm = this.$("#verification-form");
        this.$txtVerificationCode = this.$("#txtVerificationCode");
        this.$verificationMessage = this.$("#verification-code-message");
        this.$userId = this.$("#UserId");
        this.$messengerAccount = this.$("#MessengerAccount");
        this.$btnVerify = this.$("#btnVerify");
        this.$btnResend = this.$("#btnResend");
    }

    registerEvents = () => {
        this.$verificationForm.submit(this.verifyCodeEvent);
        this.$btnResend.click(this.resendEvent);
    }

    verifyCodeEvent = (e: any) => {
        e.preventDefault();

        var verificationCode = this.$txtVerificationCode.val();
        if (verificationCode.length === 0) {
            this.$txtVerificationCode.focus();
            alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.thisFieldRequired }));
            return;
        } else {
            this.$verificationMessage.html("");
        }

        if (this.isShowCaptcha) {
            if (this.$txtCaptcha.val() === "") {
                this.$txtCaptcha.focus();
                alpha.showMessage(this.$captchaMessage, new Result({ Code: 1, Message: this.resources.thisFieldRequired }));
                return;
            } else {
                this.$captchaMessage.html("");
            }
        }

        this.$.blockUI();
        var userId = this.$userId.val();
        var account = this.$messengerAccount.val();
        var code = this.$txtVerificationCode.val();

        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/VerifyVerificationCode",
            data: { userId: userId, messengerAccount: account, verificationCode: code, messengerType: 1, isShowCaptcha: this.isShowCaptcha },
            dataType: "text",
            success: this.onVerifySuccess,
            complete: this.onComplete,
        });
    }

    onVerifySuccess = (data: any) => {
        var result = this.safelyParseJSON(data);

        if (result) {
            if (result.IsOk) {
                parent.window.location.href = parent.window.location.href;
            }

            if (result.IsVerificationCodeExpired) {
                alpha.showMessage(this.$verificationMessage, new Result({ Code: 3, Message: this.resources.thisFieldRequired }));
                return;
            }

            if (result.IsWrongVerificationCode) {
                alpha.showMessage(this.$verificationMessage, new Result({ Code: 4, Message: this.resources.thisFieldRequired }));
                return;
            }

            alpha.showMessage(this.$verificationMessage, new Result({ Code: result.Code, Message: result.Message }));
            return;
        }

        this.$verificationSection.replaceWith(data);
        this.cachedDOMElements();
        this.registerEvents();

        this.isShowCaptcha = this.convertAttributeToBool(this.$("#IsShowCaptcha").val());

        if (this.isShowCaptcha) {
            this.cachedCaptchaDOMElements();
            this.registerCaptchaEvents();
            this.$captcha.show();
        }
    }

    resendEvent = (e: any) => {
        e.preventDefault();

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/ResendVerificationCode",
            dataType: "json",
            success: this.onVerifySuccess,
            complete: this.onComplete,
        });
    }

    cachedCaptchaDOMElements = () => {
        var captchaId = this.$("#CaptchaId").val();
        this.$captcha = this.$("#" + captchaId);
        this.$captchaRefreshIcon = this.$("#captcha-refresh-icon");
        this.$txtCaptcha = this.$("#txtCaptcha");
        this.$captchaMessage = this.$("#captcha-message");

        var imageElementId = this.$("#ModelImageElementId").val();
        this.$modelImageElement = this.$("#" + imageElementId);
        this.$modelTokenParameterName = this.$("#ModelTokenParameterName");

        var tokenElementId = this.$("#ModelTokenElementId").val();
        this.$modelTokenElement = this.$("#" + tokenElementId);
        this.$modelRefreshUrl = this.$("#ModelRefreshUrl");
    }

    registerCaptchaEvents = () => {
        this.$captchaRefreshIcon.click(this.refreshCaptchaEvent);
        this.$modelImageElement.click(this.refreshCaptchaEvent);
    }

    refreshCaptchaEvent = (e: any) => {
        var self = this;
        var tokenParams = {};
        tokenParams[this.$modelTokenParameterName.val()] = this.$modelTokenElement.val();

        var url = this.$modelRefreshUrl.val();
        this.$.post(url, tokenParams)
            .always(function (result) {
                eval(result.responseText);
                self.$captcha.show();
            });

        return false;
    }
}