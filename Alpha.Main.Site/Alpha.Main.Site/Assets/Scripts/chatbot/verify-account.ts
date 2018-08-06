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
    imageElementId: string;
    tokenElementId: string;

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();

        this.isShowCaptcha = this.convertAttributeToBool(this.$("#IsShowCaptcha").val());

        if (this.isShowCaptcha) {
            this.cachedCaptchaDOMElements();
            this.registerCaptchaEvents();
            this.$captcha.show();
        }
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
        this.$txtVerificationCode.keypress((event) => {
            if (event.keyCode === 13) {
                this.$btnVerify.click();
            }
        });

        this.$btnVerify.click(this.verifyCodeEvent);
        this.$btnResend.click(this.resendEvent);
    }

    cachedCaptchaDOMElements = () => {
        var captchaId = this.$("#CaptchaId").val();
        this.$captcha = this.$("#" + captchaId);
        this.$captchaRefreshIcon = this.$("#captcha-refresh-icon");
        this.$txtCaptcha = this.$("#txtCaptcha");
        this.$captchaMessage = this.$("#captcha-message");

        this.imageElementId = this.$("#ModelImageElementId").val();
        this.$modelImageElement = this.$("#" + this.imageElementId);
        this.$modelTokenParameterName = this.$("#ModelTokenParameterName");

        this.tokenElementId = this.$("#ModelTokenElementId").val();
        this.$modelTokenElement = this.$("#" + this.tokenElementId);
        this.$modelRefreshUrl = this.$("#ModelRefreshUrl");
    }

    registerCaptchaEvents = () => {
        this.$captchaRefreshIcon.click(this.refreshCaptchaEvent);
        this.$modelImageElement.click(this.refreshCaptchaEvent);
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
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/VerifyVerificationCode",
            data: this.$verificationForm.serialize(),
            dataType: "text",
            success: this.onVerifySuccess,
            complete: this.onComplete,
        });
    }

    onVerifySuccess = (data: any) => {
        var result = this.safelyParseJSON(data);

        if (result) {
            if (result.IsOk) {
                setTimeout(() => {
                    parent.window.location.href = parent.window.location.href;
                }, 2000);
                return;
            } else {
                if (result.IsNotAuthorized) {
                    this.bootbox.alert(result.Message);
                    return;
                }

                if (this.isShowCaptcha) {
                    var captchaInfo = result.CaptchaInfo;
                    this.$modelImageElement.attr('src', captchaInfo[this.imageElementId]);
                    this.$modelTokenElement.attr('value', captchaInfo[this.tokenElementId]);
                }

                if (result.IsCaptchaInvalid) {
                    alpha.showMessage(this.$captchaMessage, new Result({ Code: result.Code, Message: result.Message }));
                } else {
                    alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.verificationCodeIsInvalidOrExpired }));
                }

                return;
            }
        }

        this.$verificationSection.replaceWith(data);
        this.init();
        alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.verificationCodeIsInvalidOrExpired }))

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
        var userId = this.$userId.val();
        var account = this.$messengerAccount.val();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/ResendVerificationCode",
            data: { userId: userId, messengerUsername: account },
            dataType: "json",
            success: this.onResendSuccess,
            complete: this.onComplete,
        });
    }

    onResendSuccess = (data: any) => {
        if (data && data.IsNotAuthorized) {
            this.bootbox.alert(data.Message);
            return;
        }
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